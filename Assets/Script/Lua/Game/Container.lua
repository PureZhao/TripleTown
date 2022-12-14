local Class = require('Core.Class')
local CSType = require('Core.CSType')
local LuaBehaviour = require('Core.LuaBehaviour')
local ResManager = require('Game.Manager.ResManager')
local ResConst = require('Game.Const.ResConst')
local Coroutine = require('Core.Coroutine')

---@class Container : LuaBehaviour
local Container = Class('Container', LuaBehaviour)

Container.Instance = nil

function Container:__Define()
    self.row = CSType.Int32
    self.col = CSType.Int32
end

function Container:__init()
    -- 单例
    if not Container.Instance then
        Container.Instance = self
    else
        CSE.GameObject.Destroy(self.gameObject)
    end
    ---@type table<number, table<number, Element>>
    self.elements = {}
    self.lastSelected = nil
    self.townCount = 0
    self:_ResetColumnLack()
end

function Container:TownCountMinus()
    self.townCount = self.townCount - 1
end

function Container:_ResetColumnLack()
    self.columnLacks = {}
    for i = 1, self.col do
        self.columnLacks[i] = 0
    end
end

function Container:Generate()
    local routine = Coroutine.Create(function ()
        -- 设置随机种子
        math.randomseed(os.time())
        local counter = 0
        for row = 1, self.row do
            self.elements[row] = {}
            local rowElements = self.elements[row]
            for col = 1, self.col do
                local assetPath, id = table.randomIn(ResConst.Elements)
                ResManager.LoadGameObject(assetPath, nil, nil, function (go)
                    ---@type Element
                    local elementLua = LuaBehaviour.GetLua(go)
                    elementLua:SetPos(row, col)
                    rowElements[col] = elementLua
                    counter = counter + 1
                end)
            end
        end
        -- 等待加载完成
        coroutine.yield(CSE.WaitUntil(function ()
            return counter == self.row * self.col
        end))
        logInfo("Generate Over")
        -- 再等两秒 等待所有Element初始化完成
        coroutine.yield(CSE.WaitForSeconds(2))
        -- self:_CheckAll()

    end)
    self.host:StartCoroutine(routine)
end

function Container:AddToSelect(row, col)
    if not self.lastSelected then
        logInfo("Insert")
        -- 没有就直接插入
        self.lastSelected = {r = row, c = col}
    else
        local r = self.lastSelected.r
        local c = self.lastSelected.c
        if r == row and c == col then
            logInfo("Resume")
            -- 相同就置空，并回到初始状态
            self.elements[r][c]:BeOnSeleted(false)
            self.lastSelected = nil
        elseif (r == row and c ~= col and math.abs(c - col) == 1) or (r ~= row and c == col and math.abs(r - row) == 1) then
            logInfo("Swap")
            -- 相邻就交换，并检查是否可以消除
            local routine = Coroutine.Create(bind(self._SwapCoroutine, self), r, c, row, col)
            self.host:StartCoroutine(routine)
            self.lastSelected = nil
        else
            logInfo("Click Far")
            -- 恢复上一个的状态
            self.elements[r][c]:BeOnSeleted(false)
            -- 将该加入
            self.lastSelected = {r = row, c = col}
            self.elements[row][col]:BeOnSeleted(true)
        end
    end
end

function Container:Shuffle()
    
end

---@param r1 number
---@param c1 number
---@param r2 number
---@param c2 number
function Container:_SwapCoroutine(r1, c1, r2, c2)
    local e1 = self.elements[r1][c1]
    local e2 = self.elements[r2][c2]
    local pos1 = e1.transform.position
    local pos2 = e2.transform.position
    -- 播放交换动画
    e2.transform:DOMove(pos1, 1)
    e1.transform:DOMove(pos2, 1)
    -- 等待播放完毕
    coroutine.yield(CSE.WaitForSeconds(1.1))
    -- 交换
    self.elements[r1][c1], self.elements[r2][c2] = e2, e1
    -- 检查并执行消除
    self:_ResetColumnLack()
    self.townCount = 0
    local townList, count = self:_CheckUnit(r1, c1, self.elements[r1][c1].type)
    self.townCount = self.townCount + count
    self:_DoTown(townList)
    townList, count = self:_CheckUnit(r2, c2, self.elements[r2][c2].type)
    self.townCount = self.townCount + count
    self:_DoTown(townList)
    -- 等待动画播放完毕
    coroutine.yield(CSE.WaitUntil(function ()
        return self.townCount == 0
    end))
    logInfo("ppppppppppppppppppppppp")
end

---@param row number
---@param col number
---@param type CS.GameCore.ElementType
---@return table, number
function Container:_CheckUnit(row, col, type)
    local townList = {}
    local rowCount = 0
    local rowCheckList = {}
    local colCount = 0
    local colCheckList = {}
    --- 不把检查的点包括进去，不然还要去重
    -- 列方向
    local up = row - 1
    while up > 0 do
        if self.elements[up][col].type == type then
            table.insert(colCheckList, {up, col})
            colCount = colCount + 1
        else
            break
        end
        up = up - 1
    end
    local down = row + 1
    while down <= self.row do
        if self.elements[down][col].type == type then
            table.insert(colCheckList, {down, col})
            colCount = colCount + 1
        else
            break
        end
        down = down + 1
    end
    -- 行方向
    local left = col - 1
    while left > 0 do
        if self.elements[row][left].type == type then
            table.insert(rowCheckList, {row, left})
            rowCount = rowCount + 1
        else
            break
        end
        left = left - 1
    end
    local right = col + 1
    while right <= self.col do
        if self.elements[row][right].type == type then
            table.insert(rowCheckList, {row, right})
            rowCount = rowCount + 1
        else
            break
        end
        right = right + 1
    end
    -- 合并
    if colCount >= 2 then
        townList = table.merge(townList, colCheckList)
    end
    if rowCount >= 2 then
        townList = table.merge(townList, rowCheckList)
    end
    -- 如果有需要消除的就将自身加入
    local totalCount = table.len(townList)
    if totalCount > 0 then
        table.insert(townList, {row, col})
        totalCount = totalCount + 1
    end
    return townList, totalCount
end

function Container:_CheckAll()
    local function find(t, value)
        for _, v in pairs(t) do
            if v[1] == value[1] and v[2] == value[2] then
                return true
            end
        end
        return false
    end
    logInfo("Check All")
    local rowCheckList = {}
    local colCheckList = {}
    for r = 1, self.row do
        rowCheckList = table.merge(rowCheckList, self:_CheckRow(r))
    end

    for c = 1, self.col do
        colCheckList = table.merge(colCheckList, self:_CheckCol(c))
    end

    for _, v in pairs(rowCheckList) do
        if not find(colCheckList, v) then
            table.insert(colCheckList, v)
        end
    end

    self:_DoTown(colCheckList)

end

---@param row number
---@return table
function Container:_CheckRow(row)
    local townList = {}
    local count = 1
    local list = {}
    list[count] = {row, 1}
    local type = self.elements[row][1].type
    for c = 2, self.col do
        if self.elements[row][c].type == type then
            count = count + 1
            list[count] = {row, c}
        else
            if count >= 3 then
                townList = table.merge(townList, list)
            end
            type = self.elements[row][c].type
            table.clear(list)
            count = 1
            list[count] = {row, c}
        end
    end
    if count >= 3 then
        townList = table.merge(townList, list)
    end
    return townList
end

---@param col number
---@return table
function Container:_CheckCol(col)
    local townList = {}
    local count = 1
    local list = {}
    list[count] = {1, col}
    local type = self.elements[1][col].type
    for r = 2, self.col do
        if self.elements[r][col].type == type then
            count = count + 1
            list[count] = {r, col}
        else
            if count >= 3 then
                townList = table.merge(townList, list)
            end
            type = self.elements[r][col].type
            table.clear(list)
            count = 1
            list[count] = {r, col}
        end
    end
    if count >= 3 then
        townList = table.merge(townList, list)
    end
    return townList
end

---@table
function Container:_DoTown(elements)
    for _, v in pairs(elements) do
        local r = v[1]
        local c = v[2]
        local element = self.elements[r][c]
        self.elements[r][c] = nil
        element:PlayTown()
    end
end
return Container
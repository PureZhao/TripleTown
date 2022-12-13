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

---@param checkList table
function Container:_CheckTown(checkList)
    if not checkList then return end
    -- 标记
    local len = table.len(checkList)
    if len >= 3 then
        for _, v in pairs(checkList) do
            self.elements[v.r][v.c]:PlayTown()
        end
    end
end

function Container:_IsDead()
    
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
    -- 检查消除
    local townList = self:_CheckUnit(r1, c1, self.elements[r1][c1].type)
    logInfo(#townList)
    for k, v in pairs(townList) do
        self.elements[v.r][v.c]:PlayTown()
    end
    townList = self:_CheckUnit(r2, c2, self.elements[r2][c2].type)
    logInfo(#townList)
    for k, v in pairs(townList) do
        self.elements[v.r][v.c]:PlayTown()
    end
    coroutine.yield(CSE.WaitForSeconds(5))
end

---@param row number
---@param col number
---@param type CS.GameCore.ElementType
---@return table
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
            table.insert(colCheckList, {r = up, c = col})
            colCount = colCount + 1
        else
            break
        end
        up = up - 1
    end
    local down = row + 1
    while down <= self.row do
        if self.elements[down][col].type == type then
            table.insert(colCheckList, {r = down, c = col})
            colCount = colCount + 1
        else
            break
        end
        down = down + 1
    end
    if colCount >= 2 then
        townList = table.merge(townList, colCheckList)
    end
    -- 行方向
    local left = col - 1
    while left > 0 do
        if self.elements[row][left].type == type then
            table.insert(rowCheckList, {r = row, c = left})
            rowCount = rowCount + 1
        else
            break
        end
        left = left - 1
    end
    local right = col + 1
    while right <= self.col do
        if self.elements[row][right].type == type then
            table.insert(rowCheckList, {r = row, c = right})
            rowCount = rowCount + 1
        else
            break
        end
        right = right + 1
    end
    if rowCount >= 2 then
        townList = table.merge(townList, colCheckList)
    end
    if table.len(townList) > 0 then
        table.insert(townList, {r = row, c = col})
    end
    return townList
end

return Container
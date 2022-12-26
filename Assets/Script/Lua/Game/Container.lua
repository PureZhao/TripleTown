local Class = require('Core.Class')
local CSType = require('Core.CSType')
local LuaBehaviour = require('Core.LuaBehaviour')
local ResManager = require('Game.Manager.ResManager')
local ResConst = require('Game.Const.ResConst')
local Coroutine = require('Core.Coroutine')
local CommonUtil = require('Game.Util.CommonUtil')
local yield = coroutine.yield

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
    self.elementMatrix = {}
    self.lastSelected = nil
    self.totalCount = self.row * self.col
    self.tweenCount = 0
    self.columnLacks = {} -- 存储每列缺少的元素个数
    self.shufflePositionSequenceCache = nil
    self.shuffleElementSequenceCache = nil
    self:_ResetColumnLack()
end

function Container:TweenCountMinus()
    self.tweenCount = self.tweenCount - 1
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
            for col = 1, self.col do
                local assetPath, id = table.randomIn(ResConst.Elements)
                ResManager.LoadGameObject(assetPath, nil, nil, function (go)
                    go:SetParent(self.transform)
                    ---@type Element
                    local elementLua = LuaBehaviour.GetLua(go)
                    elementLua:SetPos(row, col)
                    self:_AddIntoElementTable(elementLua)
                    counter = counter + 1
                end)
            end
        end
        -- 等待加载完成
        yield(CSE.WaitUntil(function () return counter == self.totalCount end))
        logInfo("Generate Over")
    end)
    self.host:StartCoroutine(routine)
end

---@param element Element
function Container:_AddIntoElementTable(element)
    local r, c = element.row, element.col
    -- 加到矩阵
    if not self.elementMatrix[r] then
        self.elementMatrix[r] = {}
    end
    self.elementMatrix[r][c] = element
end

function Container:_RemoveElementFromTable(element)
    local r, c = element.row, element.col
    self.elementMatrix[r][c] = nil
end

function Container:DOJudge(row, col)
    if not self.lastSelected then
        -- 没有就直接插入
        self.lastSelected = {row, col}
    else
        local r = self.lastSelected[1]
        local c = self.lastSelected[2]
        if r == row and c == col then
            -- 相同就置空，并回到初始状态
            self.elementMatrix[r][c]:BeOnSeleted(false)
            self.lastSelected = nil
        elseif (r == row and math.abs(c - col) == 1) or (c == col and math.abs(r - row) == 1) then
            -- 相邻就交换，并检查是否可以消除
            local routine = Coroutine.Create(bind(self._SwapCoroutine, self), r, c, row, col)
            self.host:StartCoroutine(routine)
            self.lastSelected = nil
        else
            -- 恢复上一个的状态
            self.elementMatrix[r][c]:BeOnSeleted(false)
            -- 将该加入
            self.lastSelected = {row, col}
            self.elementMatrix[row][col]:BeOnSeleted(true)
        end
    end
end

---@param r1 number
---@param c1 number
---@param r2 number
---@param c2 number
function Container:_SwapCoroutine(r1, c1, r2, c2)
    local e1 = self.elementMatrix[r1][c1]
    local e2 = self.elementMatrix[r2][c2]
    self.elementMatrix[r1][c1] = e2
    self.elementMatrix[r2][c2] = e1
    e1:BeOnSeleted(false)
    e2:BeOnSeleted(false)
    self.tweenCount = 2
    e1:SetPos(r2, c2)
    e2:SetPos(r1, c1)
    -- 等待播放完毕
    yield(CSE.WaitUntil(function () return self.tweenCount == 0 end))
    -- 检查并执行消除
    local list1, count1 = self:_CheckUnit(r1, c1)
    local list2, count2 = self:_CheckUnit(r2, c2)
    self.tweenCount = count1 + count2
    if self.tweenCount == 0 then
        -- 没有消除就交换回去
        self.elementMatrix[r1][c1] = e1
        self.elementMatrix[r2][c2] = e2
        self.tweenCount = 2
        e1:SetPos(r1, c1)
        e2:SetPos(r2, c2)
        yield(CSE.WaitUntil(function () return self.tweenCount == 0 end))
    else
        local townList = table.merge(list1, list2)
        self:_DoTown(townList)
        -- 等待动画播放完毕
        yield(CSE.WaitUntil(function () return self.tweenCount == 0 end))
        self:_ElementDropDown()
        yield(CSE.WaitUntil(function () return self.tweenCount == 0 end))
        self:_ResumeColumn()
        yield(CSE.WaitUntil(function () return self.tweenCount == 0 end))
        -- 开始循环检查 自动消除
        while true do
            townList = self:_CheckAll()
            self.tweenCount = table.len(townList)
            if self.tweenCount == 0 then
                break
            end
            self:_ResetColumnLack()
            self:_DoTown(townList)
            yield(CSE.WaitUntil(function () return self.tweenCount == 0 end))
            self:_ElementDropDown()
            yield(CSE.WaitUntil(function () return self.tweenCount == 0 end))
            self:_ResumeColumn()
            yield(CSE.WaitUntil(function () return self.tweenCount == 0 end))
        end
        logInfo("Check Over")
        if self:_CheckIsDead() then
        -- if true then
            self:DOShuffle()
        end
    end
end

---@param row number
---@param col number
---@return table, number
function Container:_CheckUnit(row, col)
    local townList = {}
    local type = self.elementMatrix[row][col].type
    local rowCount = 0
    local rowCheckList = {}
    local colCount = 0
    local colCheckList = {}
    --- 不把检查的点包括进去，不然还要去重
    -- 列方向
    local up = row - 1
    while up > 0 do
        if self.elementMatrix[up][col].type == type then
            table.insert(colCheckList, {up, col})
            colCount = colCount + 1
        else
            break
        end
        up = up - 1
    end
    local down = row + 1
    while down <= self.row do
        if self.elementMatrix[down][col].type == type then
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
        if self.elementMatrix[row][left].type == type then
            table.insert(rowCheckList, {row, left})
            rowCount = rowCount + 1
        else
            break
        end
        left = left - 1
    end
    local right = col + 1
    while right <= self.col do
        if self.elementMatrix[row][right].type == type then
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
    local rowCheckList = {}
    local colCheckList = {}
    for r = 1, self.row do
        rowCheckList = table.merge(rowCheckList, self:_CheckRow(r))
    end

    for c = 1, self.col do
        colCheckList = table.merge(colCheckList, self:_CheckCol(c))
    end
    -- 去重合并
    for _, v in pairs(rowCheckList) do
        if not find(colCheckList, v) then
            table.insert(colCheckList, v)
        end
    end

    return colCheckList

end

---@param row number
---@return table
function Container:_CheckRow(row)
    local townList = {}
    local count = 1
    local list = {}
    list[count] = {row, 1}
    local type = self.elementMatrix[row][1].type
    for c = 2, self.col do
        if self.elementMatrix[row][c].type == type then
            count = count + 1
            list[count] = {row, c}
        else
            if count >= 3 then
                townList = table.merge(townList, list)
            end
            type = self.elementMatrix[row][c].type
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
    local type = self.elementMatrix[1][col].type
    for r = 2, self.col do
        if self.elementMatrix[r][col].type == type then
            count = count + 1
            list[count] = {r, col}
        else
            if count >= 3 then
                townList = table.merge(townList, list)
            end
            type = self.elementMatrix[r][col].type
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
        local element = self.elementMatrix[r][c]
        self:_RemoveElementFromTable(element)
        element:PlayTown()
    end
end

function Container:_ElementDropDown()
    self:_ResetColumnLack()
    self.tweenCount = 0
    for c = 1, self.col do
        local count = 0
        for r = 1, self.row do
            local element = self.elementMatrix[r][c]
            if element then
                count = count + 1
                if count ~= element.row then
                    self:_RemoveElementFromTable(element)
                    self.tweenCount = self.tweenCount + 1
                    element:SetPos(count, c)
                    self:_AddIntoElementTable(element)
                end
            end
        end
        self.columnLacks[c] = self.row - count
    end
end

function Container:_ResumeColumn()
    math.randomseed(os.time())
    self.tweenCount = 0
    for col, count in pairs(self.columnLacks) do
        self.tweenCount = self.tweenCount + count
        for i = 1, count do
            local assetPath = table.randomIn(ResConst.Elements)
            local x = -4.5 + (col - 1)
            local pos = CSE.Vector3(x, 6, 0)
            local row = self.row - count + i
            ResManager.LoadGameObject(assetPath, pos, CSE.Quaternion.identity, function (go)
                ---@type Element
                local lua = LuaBehaviour.GetLua(go)
                if lua then
                    lua.row = -1
                    lua.col = -1
                    lua:BeOnSeleted(false)
                    lua:SetPos(row, col)
                    self:_AddIntoElementTable(lua)
                end
            end)
        end
    end
end

function Container:DODismissRandomLine()
    
end

function Container:DODismissRandomType()
    
end

---@return boolean
function Container:_CheckIsDead()
    for i = 1, self.row do
        for j = 1, self.col do
            local type = self.elementMatrix[i][j].type
            local townPointCollection = self:_GenTownPointCollection(i, j)
            for _, collection in pairs(townPointCollection) do
                local r1, c1 = collection[1][1], collection[1][2]
                local r2, c2 = collection[2][1], collection[2][2]
                print(i, j)
                print(r1, c1)
                print(r2, c2)
                print("---------------")
                if self.elementMatrix[r1][c1].type == type and self.elementMatrix[r2][c2].type == type then
                    return false
                end
            end
        end
    end
    return true
end

function Container:DOShuffle()
    local routine = Coroutine.Create(bind(self._ShuffleCoroutine, self))
    self.host:StartCoroutine(routine)
end



function Container:_ShuffleCoroutine()
    -- 保证至少有一处可以消除
    local eleSequence = self:_GenShuffleElementSequence()
    -- 直接从缓存里面拿位置
    local posSequence = self:_GenShufflePositionSequence()
    self.elementMatrix = {}
    self.tweenCount = self.totalCount
    for i = 1, self.totalCount do
        local e = eleSequence[i]
        local pos = posSequence[i]
        e:SetPos(pos[1], pos[2])
        self:_AddIntoElementTable(e)
    end
    yield(CSE.WaitUntil(function() return self.tweenCount == 0 end))
end

function Container:_GenShuffleElementSequence()
    local t = {}
    local cands = {}
    for r = 1, self.row do
        for c = 1, self.col do
            local element = self.elementMatrix[r][c]
            local type = element.type
            if not t[type] then
                t[type] = {0, {}}
            end
            t[type][1] = t[type][1] + 1
            table.insert(t[type][2], element)
            if t[type][1] > 2 then
                table.insert(cands, type)
            end
        end
    end
    local targetType = table.randomIn(cands)
    math.randomseed(os.time())
    local sequence = t[targetType][2]
    t[targetType] = nil
    for type, content in pairs(t) do
        sequence = table.merge(sequence, content[2])
    end
    return sequence
end

--- 前三个位置保证有可消除的位置
function Container:_GenShufflePositionSequence()
    local m = CommonUtil.GenerateMatrix(self.row, self.col, false)
    -- 保证一定有位置可以消除
    local r = math.random(self.row)
    local c = math.random(self.col)
    local ensurePoints = table.randomIn(self:_GenTownPointCollection(r, c))
    table.insert(ensurePoints, {r, c})
    for _, point in pairs(ensurePoints) do
        m[point[1]][point[2]] = true
    end
    local othersPoints = {}
    for i = 1, self.row do
        for j = 1, self.col do
            if not m[i][j] then
                table.insert(othersPoints, {i, j})
            end
        end
    end
    othersPoints = table.shuffle(othersPoints)
    local res = table.merge(ensurePoints, othersPoints)
    return res
end

--- 只检测另外两个点
---@param r number
---@param c number
function Container:_GenTownPointCollection(r, c)
    local collections = {
        -- Up
        {{r + 1, c}, {r + 2, c - 1}},
        {{r + 1, c}, {r + 3, c}},
        {{r + 1, c}, {r + 2, c + 1}},
        {{r + 2, c}, {r + 1, c - 1}},
        {{r + 2, c}, {r + 1, c + 1}},
        -- down
        {{r - 1, c}, {r - 2, c - 1}},
        {{r - 1, c}, {r - 3, c}},
        {{r - 1, c}, {r - 2, c + 1}},
        {{r - 2, c}, {r - 1, c - 1}},
        {{r - 2, c}, {r - 1, c + 1}},
        -- left
        {{r, c - 1}, {r + 1, c - 2}},
        {{r, c - 1}, {r, c - 3}},
        {{r, c - 1}, {r - 1, c - 2}},
        {{r, c - 2}, {r - 1, c - 1}},
        {{r, c - 2}, {r + 1, c - 1}},
        -- right
        {{r, c + 1}, {r + 1, c + 2}},
        {{r, c + 1}, {r, c + 3}},
        {{r, c + 1}, {r - 1, c + 2}},
        {{r, c + 2}, {r - 1, c + 1}},
        {{r, c + 2}, {r + 1, c + 1}},
    }
    -- 检查合法性(过滤)
    local res = {}
    for _, collection in pairs(collections) do
        local isValid = true
        for _, point in pairs(collection) do
            -- 如果超出矩阵范围，排除
            if point[1] < 1 or point[1] > self.row or point[2] < 1 or point[2] > self.col then
                isValid = false
                break
            end
        end
        if isValid then
            table.insert(res, collection)
        end
    end
    return res
end

return Container
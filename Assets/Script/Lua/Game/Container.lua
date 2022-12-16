local Class = require('Core.Class')
local CSType = require('Core.CSType')
local LuaBehaviour = require('Core.LuaBehaviour')
local ResManager = require('Game.Manager.ResManager')
local ResConst = require('Game.Const.ResConst')
local Coroutine = require('Core.Coroutine')

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
    self.elements = {}
    self.lastSelected = nil
    self.tweenCount = 0
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
            self.elements[row] = {}
            local rowElements = self.elements[row]
            for col = 1, self.col do
                local assetPath, id = table.randomIn(ResConst.Elements)
                ResManager.LoadGameObject(assetPath, nil, nil, function (go)
                    go:SetParent(self.transform)
                    ---@type Element
                    local elementLua = LuaBehaviour.GetLua(go)
                    elementLua:SetPos(row, col)
                    rowElements[col] = elementLua
                    counter = counter + 1
                end)
            end
        end
        -- 等待加载完成
        yield(CSE.WaitUntil(function () return counter == self.row * self.col end))
        logInfo("Generate Over")
        -- 再等两秒 等待所有Element初始化完成
        yield(CSE.WaitForSeconds(2))
    end)
    self.host:StartCoroutine(routine)
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
            self.elements[r][c]:BeOnSeleted(false)
            self.lastSelected = nil
        elseif (r == row and math.abs(c - col) == 1) or (c == col and math.abs(r - row) == 1) then
            -- 相邻就交换，并检查是否可以消除
            local routine = Coroutine.Create(bind(self._SwapCoroutine, self), r, c, row, col)
            self.host:StartCoroutine(routine)
            self.lastSelected = nil
        else
            -- 恢复上一个的状态
            self.elements[r][c]:BeOnSeleted(false)
            -- 将该加入
            self.lastSelected = {row, col}
            self.elements[row][col]:BeOnSeleted(true)
        end
    end
end

---@param r1 number
---@param c1 number
---@param r2 number
---@param c2 number
function Container:_SwapCoroutine(r1, c1, r2, c2)
    local e1 = self.elements[r1][c1]
    local e2 = self.elements[r2][c2]
    self.elements[r1][c1] = e2
    self.elements[r2][c2] = e1
    e1:BeOnSeleted(false)
    e2:BeOnSeleted(false)
    self.tweenCount = 2
    e1:MoveTo(r2, c2)
    e2:MoveTo(r1, c1)
    -- 等待播放完毕
    yield(CSE.WaitUntil(function () return self.tweenCount == 0 end))
    -- 检查并执行消除
    local list1, count1 = self:_CheckUnit(r1, c1)
    local list2, count2 = self:_CheckUnit(r2, c2)
    self.tweenCount = count1 + count2
    if self.tweenCount == 0 then
        -- 没有消除就交换回去
        self.elements[r1][c1] = e1
        self.elements[r2][c2] = e2
        self.tweenCount = 2
        e1:MoveTo(r1, c1)
        e2:MoveTo(r2, c2)
        yield(CSE.WaitUntil(function () return self.tweenCount == 0 end))
    else
        local townList = table.merge(list1, list2)
        self:_ResetColumnLack()
        self:_DoTown(townList)
        -- 等待动画播放完毕
        yield(CSE.WaitUntil(function () return self.tweenCount == 0 end))
        -- 检查每列缺少的元素个数并补齐
        self:_ResumeElementMatrix()
        yield(CSE.WaitUntil(function () return self.tweenCount == 0 end))
        self.tweenCount = 0
        for _, count in pairs(self.columnLacks) do
            self.tweenCount = self.tweenCount + count
        end
        math.randomseed(os.time())
        for col, count in pairs(self.columnLacks) do
            self:_RestoreColumn(col, count)
        end
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
            self:_ResumeElementMatrix()
            yield(CSE.WaitUntil(function () return self.tweenCount == 0 end))
            self.tweenCount = 0
            for _, count in pairs(self.columnLacks) do
                self.tweenCount = self.tweenCount + count
            end
            math.randomseed(os.time())
            for col, count in pairs(self.columnLacks) do
                self:_RestoreColumn(col, count)
            end
            yield(CSE.WaitUntil(function () return self.tweenCount == 0 end))
        end
        logInfo("Check Over")
        if self:_CheckIsDead() then
            local routine = Coroutine.Create(bind(self._ShuffleCoroutine, self))
            self.host:StartCoroutine(routine)
        end
    end
end

---@param row number
---@param col number
---@return table, number
function Container:_CheckUnit(row, col)
    local townList = {}
    local type = self.elements[row][col].type
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
        self.columnLacks[c] = self.columnLacks[c] + 1
        element:PlayTown()
    end
end

function Container:_ResumeElementMatrix()
    self.tweenCount = 0
    local needMove = {}
    for col, count in pairs(self.columnLacks) do
        if count ~= 0 then
            local exists = {}
            -- 收集列所有还存在的元素，并置空该列
            for r = 1, self.row do
                local element = self.elements[r][col]
                self.elements[r][col] = nil
                if element then
                    table.insert(exists, element)
                end
            end
            -- 计算需要播放移动动画的元素个数
            for k, v in pairs(exists) do
                self.elements[k][col] = v
                if v.row ~= k then
                    self.tweenCount = self.tweenCount + 1
                    table.insert(needMove, {k, col, v})
                end
            end
        end
    end
    for _, t in pairs(needMove) do
        local r = t[1]
        local c = t[2]
        local e = t[3]
        e:MoveTo(r, c)
    end
end

function Container:_RestoreColumn(col, count)
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
                self.elements[row][col] = lua
                lua:MoveTo(row, col)
            end
        end)
    end
end

---@return boolean
function Container:_CheckIsDead()
    for i = 1, self.row do
        for j = 1, self.col do
            -- 上
            if self:_CheckPreTown({i, j}, {i + 1, j}, {{i + 2, j - 1}, {i + 3, j}, {i + 2, j + 1}}) then return false end
            -- 下
            if self:_CheckPreTown({i, j}, {i - 1, j}, {{i - 2, j - 1}, {i - 3, j}, {i - 2, j + 1}}) then return false end
            -- 左
            if self:_CheckPreTown({i, j}, {i, j - 1}, {{i + 1, j - 2}, {i, j - 3}, {i - 1, j - 2}}) then return false end
            -- 右
            if self:_CheckPreTown({i, j}, {i, j + 1}, {{i + 1, j + 2}, {i, j + 3}, {i - 1, j + 2}}) then return false end
        end
    end
    return true
end

---@param cur table
---@param next table
---@param threeCheckPos table<any, table>
---@return boolean
function Container:_CheckPreTown(cur, next, threeCheckPos)
    local function IsValid(pos)
        return pos[1] >= 1 and pos[1] <= self.row and pos[2] >= 1 and pos[2] <= self.col
    end
    local type = self.elements[cur[1]][cur[2]].type
    -- 检查相邻
    local nextRow = self.elements[next[1]]
    if not nextRow then return false end
    local nextElement = nextRow[next[2]]
    -- 相邻不存在 或者 相邻的元素类型不同 则不能消除
    if not nextElement or nextElement.type ~= type then return false end
    for _, v in pairs(threeCheckPos) do
        if IsValid(v) then
            local r = v[1]
            local c = v[2]
            local element = self.elements[r][c]
            if element and element.type == type then
                return true
            end
        end
    end
    return false
end

function Container:_ShuffleCoroutine()
    math.randomseed(os.time())
    -- 保证至少有一处可以消除
    local r = math.random(self.row)
    local c = math.random(self.col)
    local targetType = self.elements[r][c].type
    local ensure = {self.elements[r][c]}
    self.elements[r][c] = nil
    local left = {}
    local count = 2
    -- 找其他两个相同类型的元素
    for i = 1, self.row do
        for j = 1, self.col do
            local element = self.elements[i][j]
            if element then
                if count > 0 and element.type == targetType then
                    table.insert(ensure, element)
                    count = count - 1
                else
                    table.insert(left, element)
                end
            end
        end
    end
    local ensureLen = table.len(ensure)
    local leftLen = table.len(left)
    -- 计算位置
    local ensurePos, othersPos = self:_CalcPos()
    -- 生成一个新矩阵
    local matrix = {}
    for row = 1, self.row do
        matrix[row] = {}
    end
    self.tweenCount = self.row * self.col
    for i = 1, ensureLen do
        local pos = ensurePos[i]
        local element = ensure[i]
        matrix[pos[1]][pos[2]] = element
        element:MoveTo(pos[1], pos[2])
    end
    for i = 1, leftLen do
        local pos = othersPos[i]
        local element = left[i]
        matrix[pos[1]][pos[2]] = element
        element:MoveTo(pos[1], pos[2])
    end
    self.elements = matrix
    yield(CSE.WaitUntil(function() return self.tweenCount == 0 end))
end

function Container:_CalcPos()
    local function exist(t, item)
        for _, v in pairs(t) do
            if v[1] == item[1] and v[2] == item[2] then
                return true
            end
        end
        return false
    end
    local ensurePos = self:_CalcThreePos()
    local positions = {}
    for i = 1, self.row do
        for j = 1, self.col do
            if not exist(ensurePos, {i, j}) then
                table.insert(positions, {i, j})
            end
        end
    end
    positions = table.shuffle(positions)
    return ensurePos, positions
end

function Container:_CalcThreePos()
    local function IsValid(pos)
        return pos[1] >= 1 and pos[1] <= self.row and pos[2] >= 1 and pos[2] <= self.col
    end
    math.randomseed(os.time())
    -- 保证一定有位置可以消除
    local r = math.random(self.row)
    local c = math.random(self.col)
    local options = {
        -- 上
        {{r + 1, c}, {r + 2, c - 1}, {r + 3, c}, {r + 2, c + 1}},
        -- 下
        {{r - 1, c}, {r - 2, c - 1}, {r - 3, c}, {r - 2, c + 1}},
        -- 左
        {{r, c - 1}, {r - 1, c - 2}, {r, c - 3}, {r + 1, c - 2}},
        -- 右
        {{r, c + 1}, {r - 1, c + 2}, {r, c + 3}, {r + 1, c + 2}}
    }
    local candidates = {}
    for _, option in pairs(options) do
        local next = option[1]
        local count = 0
        local points = {}
        if IsValid(next) then
            for i = 2, 4 do
                if IsValid(option[i]) then
                    count = count + 1
                    table.insert(points, option[i])
                end
            end
        end
        if count > 0 then
            local elect = table.randomIn(points)
            points= {elect}
            table.insert(points, {r, c})
            table.insert(points, option[1])
            table.insert(candidates, points)
        end
    end
    return table.randomIn(candidates)
end

return Container
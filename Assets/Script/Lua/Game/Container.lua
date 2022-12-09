local Class = require('Core.Class')
local CSType = require('Core.CSType')
local LuaBehaviour = require('Core.LuaBehaviour')
local Timer = require('Game.Util.Timer')
local ResManager = require('Game.Manager.ResManager')
local ResConst = require('Game.Const.ResConst')

---@class Container : LuaBehaviour
local Container = Class('Container', LuaBehaviour)

function Container:__Define()
    self.row = CSType.Int32
    self.col = CSType.Int32
end

function Container:__init()
    -- x -4.5 ~ 2.5
    -- y -4.5 ~ 2.5
    self.elements = nil
    self.timer = Timer.New()
    self.timer:Delay(5, bind(self.Generate, self))
    self._townRowCheckList = {}
    self._townColCheckList = {}
end

function Container:Generate()
    self.elements = {}
    for row = 1, self.row do
        table.insert(self.elements, {})
        local rowElements = self.elements[row]
        for col = 1, self.col do
            local elementLua = col + 1  -- 后面换成游戏对象
            local type = LuaBehaviour.GetLua(element).type
            table.insert(rowElements, {obj = element, type = type, canTown = false})
        end
    end
end

function Container:CheckAndTown()
    for row = 1, self.row do
        for col = 1, self.col do
            -- 开始检查
            local rowCheckList, colCheckList = self:_CheckUnit(row, col)
            -- 检查完毕，开始标记
            self:_CheckTown(rowCheckList)
            self:_CheckTown(colCheckList)
        end
    end
    -- 开始播放Town效果
    for row = 1, self.row do
        for col = 1, self.col do
            local element = self.elements[row][col]
            if element.canTown then
                element:PlayTown()
            end
        end
    end
end

---@param row number
---@param col number
---@return table rowCheckList, table colCheckList
function Container:_CheckUnit(row, col)
    -- 初始化checkList
    local rowCheckList = {{r = row, c = col}}
    local colCheckList = {{r = row, c = col}}
    local curElementType = self.elements[row][col].type
    local up = row - 1
    local down = row + 1
    local left = col - 1
    local right = col + 1
    -- 向上
    while up > 0 do
        local element = self.elements[up][col]
        if element.type == curElementType then
            table.insert(colCheckList, {r = up, c = col})
        else
            break
        end
        up = up - 1
    end
    -- 向下
    while down < self.row + 1 do
        local element = self.elements[down][col]
        if element.type == curElementType then
            table.insert(colCheckList, {r = down, c = col})
        else
            break
        end
        down = down + 1
    end
    -- 向左
    while left > 0 do
        local element = self.elements[row][left]
        if element.type == curElementType then
            table.insert(rowCheckList, {r = row, c = left})
        else
            break
        end
        left = left - 1
    end
    -- 向右
    while right < self.col + 1 do
        local element = self.elements[row][right]
        if element.type == curElementType then
            table.insert(rowCheckList, {r = row, c = right})
        else
            break
        end
        right = right + 1
    end
    return rowCheckList, colCheckList
end

---@param checkList table
function Container:_CheckTown(checkList)
    if not checkList then return end
    -- 标记
    local len = table.len(checkList)
    if len >= 3 then
        for _, v in pairs(checkList) do
            self.elements[v.r][v.c].canTown = true
        end
    end
end

function Container:_IsDead()
    
end

function Container:Shuffle()
    
end

return Container
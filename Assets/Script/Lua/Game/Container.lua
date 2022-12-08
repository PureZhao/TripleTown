local Class = require('Core.Class')
local CSType = require('Core.CSType')
local LuaBehaviour = require('Core.LuaBehaviour')
local Timer = require('Game.Util.Timer')

---@class Container : LuaBehaviour
local Container = Class('Container', LuaBehaviour)

function Container:__Define()
    self.row = CSType.Int32
    self.col = CSType.Int32
end

function Container:__init()
    self.elements = nil
    self.timer = Timer.New()
    self.timer:Delay(5, bind(self.Generate, self))
    self._townRowCheckList = {}
    self._townColCheckList = {}
    -- x -4.5 ~ 2.5
    -- y -4.5 ~ 2.5
end

function Container:Generate()
    self.elements = {}
    for row = 1, self.row do
        table.insert(self.elements, {})
        for col = 1, self.col do
            local rowElements = self.elements[row]
            local element = col + 1  -- 后面换成游戏对象
            local type = LuaBehaviour.GetLua(element).type
            table.insert(rowElements, {obj = element, type = type, canTown = false})
        end
    end
end

function Container:CheckAndTown()
    for row = 1, self.row do
        for col = 1, self.col do
            -- 检查前先将行、列检查表分别置空
            self._townRowCheckList = {}
            self._townColCheckList = {}
            -- 将此刻该位置加入列表
            table.insert(self._townRowCheckList, self.elements[row][col])
            table.insert(self._townColCheckList, self.elements[row][col])
            -- 开始检查
            self:_CheckUnit(row, col)
            -- 检查完毕，开始标记
            
        end
    end
end

function Container:_CheckUnit(row, col)
    local curElementType = self.elements[row][col].type
    local up = row - 1
    local down = row + 1
    local left = col - 1
    local right = col + 1
    -- 向上
    while up > 0 do
        local element = self.elements[up][col]
        if element.type == curElementType then
            table.insert(self._townColCheckList, element)
        else
            break
        end
        up = up - 1
    end
    -- 向下
    while down < self.row + 1 do
        local element = self.elements[down][col]
        if element.type == curElementType then
            table.insert(self._townColCheckList, element)
        else
            break
        end
        down = down + 1
    end
    -- 向左
    while left > 0 do
        local element = self.elements[row][left]
        if element.type == curElementType then
            table.insert(self._townColCheckList, element)
        else
            break
        end
        left = left - 1
    end
    -- 向右
    while right < self.col + 1 do
        local element = self.elements[row][right]
        if element.type == curElementType then
            table.insert(self._townColCheckList, element)
        else
            break
        end
        right = right + 1
    end
end

function Container:Shuffle()
    
end

return Container
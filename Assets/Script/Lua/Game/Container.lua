local Class = require('Core.Class')
local CSType = require('Core.CSType')
local LuaBehaviour = require('Core.LuaBehaviour')


local Container = Class('Container', LuaBehaviour)

function Container:__Define()
    self.row = CSType.Int32
    self.col = CSType.Int32
    
end

function Container:__init()
    self.elements = {}
    -- -4.5 ~ 2.5
    -- -4.5 ~ 2.5
    -- center 1, 1
end

function Container:Generate()
    
end

function Container:CheckAndTown()
    
end

function Container:Shuffle()
    
end

return Container
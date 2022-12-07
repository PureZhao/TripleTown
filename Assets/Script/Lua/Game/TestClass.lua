local Class = require('Core.Class')
local CSType = require('Core.CSType')
local LuaBehaviour = require('Core.LuaBehaviour')

local TestClass = Class('TestClass', LuaBehaviour)

function TestClass:__init()
    self.testBind = bind(self.Shuffle, self)
    self.testBind()
end

function TestClass:Shuffle()
    logInfo("Shuffle")
end

return TestClass
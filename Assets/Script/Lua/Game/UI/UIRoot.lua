local Class = require('Core.Class')
local CSType = require('Core.CSType')
local LuaBehaviour = require('Core.LuaBehaviour')

---@class UIRoot : LuaBehaviour
local UIRoot = Class('UIRoot', LuaBehaviour)
UIRoot.Instance = nil

function UIRoot:__init()
    UIRoot.Instance = self
end

return UIRoot
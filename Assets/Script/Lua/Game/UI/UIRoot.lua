local Class = require('Core.Class')
local CSType = require('Core.CSType')
local LuaBehaviour = require('Core.LuaBehaviour')

---@class UIRoot : LuaBehaviour
local UIRoot = Class('UIRoot', LuaBehaviour)
UIRoot.Instance = nil

function UIRoot:__init()
    if not UIRoot.Instance then
        UIRoot.Instance = self
    else
        CSE.GameObject.Destroy(self.gameObject)
    end
    self.uiUpdateFuncs = {}
end

function UIRoot:UpdateUI(updateType, ...)
    for _, v in pairs(self.uiUpdateFuncs) do
        v(updateType, ...)
    end
end

function UIRoot:Registry(func)
    table.insert()
end

function UIRoot:Unregistry()
    
end

return UIRoot
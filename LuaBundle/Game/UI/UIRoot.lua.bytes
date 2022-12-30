local Class = require('Core.Class')
local CSType = require('Core.CSType')
local LuaBehaviour = require('Core.LuaBehaviour')
local UIConst      = require('Game.UI.UIConst')

---@class UIRoot : LuaBehaviour
local UIRoot = Class('UIRoot', LuaBehaviour)
UIRoot.Instance = nil

function UIRoot:__Define()
    self.displayBar = CSType.RectTransform
    self.inventoryBar = CSType.RectTransform
end

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

function UIRoot:OnGameStart(...)
    self.displayBar.gameObject:SetActive(true)
    self.inventoryBar.gameObject:SetActive(true)
    local param = {...}
    self:UpdateUI(UIConst.UIUpdateType.InitItems)
    self:OpenBars()
    self:ResetDisplay(param[1])
end

function UIRoot:OnGameEnd()
    self:CloseBars()
end

function UIRoot:OpenBars()
    self.displayBar:DOLocalMoveY(500, 1)
    self.inventoryBar:DOLocalMoveX(500, 1)
end

function UIRoot:CloseBars()
    self.displayBar:DOLocalMoveY(700, 1)
    self.inventoryBar:DOLocalMoveX(700, 1)
end

function UIRoot:ResetDisplay(timeLeft)
    self:UpdateUI(UIConst.UIUpdateType.Combo, 0)
    self:UpdateUI(UIConst.UIUpdateType.Score, 0)
    self:UpdateUI(UIConst.UIUpdateType.TimeLeft, timeLeft)
end

function UIRoot:Registry(func)
    table.insert(self.uiUpdateFuncs, func)
end

function UIRoot:Unregistry()
    
end

return UIRoot
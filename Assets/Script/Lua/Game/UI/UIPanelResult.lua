
local Class = require('Core.Class')
local LuaBehaviour = require('Core.LuaBehaviour')
local CSType = require('Core.CSType')

---@class UIPanelResult
local UIPanelResult = Class('UIBtnQuit', LuaBehaviour)

function UIPanelResult:__Define()
    self.btnRetry = CSType.Button
    self.btnQuit = CSType.Button
    self.scoreText = CSType.Text
    self.comboText = CSType.Text
end

function UIPanelResult:__init()
    self.transform.localScale = CSE.Vector3.zero
    self.transform:DOScale(CSE.Vector3.one, 1)
    self.isQuit = false
    self.btnQuit.onClick:AddListener(bind(self.OnQuitClick, self))
    self.btnRetry.onClick:AddListener(bind(self.OnRetryClick, self))
end

function UIPanelResult:OnRetryClick()
    local gameManager = require('Game.Manager.GameManager')
    gameManager:StartGame()
end

function UIPanelResult:OnQuitClick()
    if self.isQuit then return end
    self.isQuit = true
    CSE.Application.Quit()
end

function UIPanelResult:SetData(score, combo)
    self.scoreText.text = tostring(score)
    self.comboText.text = tostring(combo)
end

function UIPanelResult:__delete()
    
end

return UIPanelResult
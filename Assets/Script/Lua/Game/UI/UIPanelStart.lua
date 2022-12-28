local Class = require('Core.Class')
local CSType = require('Core.CSType')
local LuaBehaviour = require('Core.LuaBehaviour')
local Coroutine = require('Core.Coroutine')
local yield = coroutine.yield

---@class UIPanelStart : LuaBehaviour
local UIPanelStart = Class("UIPanelStart", LuaBehaviour)

function UIPanelStart:__Define()
    self.btnStart = CSType.Button
    self.bg = CSType.Image
    self.countDownText = CSType.Text
end

function UIPanelStart:__init()
    self.btnStart.onClick:AddListener(bind(self.OnStartClick, self))
end

function UIPanelStart:OnStartClick()
    local routine = Coroutine.Create(bind(self._StartCoroutine, self))
    self.host:StartCoroutine(routine)
    self.btnStart.gameObject:SetActive(false)
end

function UIPanelStart:_StartCoroutine()
    self.bg.gameObject:SetActive(true)
    self.countDownText.gameObject:SetActive(true)
    self.countDownText.text = "3"
    for i = 3, 1, -1 do
        self.bg.fillAmount = 1
        self.bg:DOFillAmount(0, 0.95)
        self.countDownText.text = tostring(i)
        yield(CSE.WaitForSeconds(1))
    end
    self.gameObject:SetActive(false)
    local gameManager = require('Game.Manager.GameManager')
    gameManager:StartGame()
end

return UIPanelStart
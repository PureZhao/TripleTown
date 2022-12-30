local Class = require('Core.Class')
local CSType = require('Core.CSType')
local LuaBehaviour = require('Core.LuaBehaviour')
local UIRoot = require('Game.UI.UIRoot')
local UIConst= require('Game.UI.UIConst')

---@class UIDisplayBar : LuaBehaviour
local UIDisplayBar = Class("UIDisplayTime", LuaBehaviour)

function UIDisplayBar:__Define()
    self.timeLeftText = CSType.Text
    self.comboText = CSType.Text
    self.scoreText = CSType.Text
end

function UIDisplayBar:__init()
    self.comboTween = nil
    self.scoreTween = nil
    -- 注册到UIRoot去
    UIRoot.Instance:Registry(bind(self._UpdateUI, self))
end

function UIDisplayBar:_UpdateUI(type, ...)
    local func = UIDisplayBar.UpdateFunc[type]
    local param = {...}
    if func then
        func(self, param[1])
    end
end

function UIDisplayBar:_UpdateCombo(combo)
    if self.comboTween then
        self.comboTween:Kill()
        self.comboTween = nil
        self.comboText.transform.localScale = CSE.Vector3.one
    end
    local text = "Combo\n" .. tostring(combo)
    self.comboTween = self.comboText.transform:DOScale(1.3, 0.1):OnComplete(function ()
        self.comboText.transform.localScale = CSE.Vector3.one
    end)
    self.comboText.text = text
end

function UIDisplayBar:_UpdateTime(time)
    local minutes = time // 60
    local seconds = time - (minutes * 60)
    self.timeLeftText.text = string.format("TimeLeft\n%02d:%02d", minutes, seconds)
end

function UIDisplayBar:_UpdateScore(score)
    if self.scoreTween then
        self.scoreTween:Kill()
        self.scoreTween = nil
        self.scoreText.transform.localScale = CSE.Vector3.one
    end
    local text = "Score\n" .. tostring(score)
    self.scoreTween = self.scoreText.transform:DOScale(1.3, 0.1):OnComplete(function ()
        self.scoreText.transform.localScale = CSE.Vector3.one
    end)
    self.scoreText.text = text
end

function UIDisplayBar:__delete()
    if self.scoreTween then
        self.scoreTween:Kill()
    end
    if self.comboTween then
        self.comboTween:Kill()
    end
    self.scoreTween = nil
    self.comboTween = nil
end

UIDisplayBar.UpdateFunc = {
    [UIConst.UIUpdateType.Combo] = UIDisplayBar._UpdateCombo,
    [UIConst.UIUpdateType.Score] = UIDisplayBar._UpdateScore,
    [UIConst.UIUpdateType.TimeLeft] = UIDisplayBar._UpdateTime,
}

return UIDisplayBar
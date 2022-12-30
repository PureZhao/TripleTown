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
    self.comboText.text = "Combo\n" .. tostring(combo)
end

function UIDisplayBar:_UpdateTime(time)
    local minutes = time // 60
    local seconds = time - (minutes * 60)
    self.timeLeftText.text = string.format("TimeLeft\n%02d:%02d", minutes, seconds)
end

function UIDisplayBar:_UpdateScore(score)
    self.scoreText.text = "Score\n" .. tostring(score)
end

UIDisplayBar.UpdateFunc = {
    [UIConst.UIUpdateType.Combo] = UIDisplayBar._UpdateCombo,
    [UIConst.UIUpdateType.Score] = UIDisplayBar._UpdateScore,
    [UIConst.UIUpdateType.TimeLeft] = UIDisplayBar._UpdateTime,

}

return UIDisplayBar
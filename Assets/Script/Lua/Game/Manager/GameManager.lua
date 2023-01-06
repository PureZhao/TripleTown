local Class = require('Core.Class')
local ResManager = require('Game.Manager.ResManager')
local ResConst = require('Game.Const.ResConst')
local Container = require('Game.Container')
local GameConst = require('Game.Const.GameConst')
local LuaBehaviour = require('Core.LuaBehaviour')
local UIRoot = require('Game.UI.UIRoot')
local LuaTimer = require('Game.Util.LuaTimer')
local UIConst = require('Game.UI.UIConst')

---@class GameManager
local GameManager = Class('GameManager')

function GameManager:__init()
    self.combo = 0
    self.score = 0
    self.time = 0
    self.container = nil
    self.timeCounter = nil
    self.allowTimeFlow = true
    ResManager.LoadGameObject(ResConst.UIRoot, CSE.Vector3.zero, CSE.Quaternion.identity)
    ResManager.LoadGameObject(ResConst.Container, CSE.Vector3.zero, CSE.Quaternion.identity, function (go)
        self.container = LuaBehaviour.GetLua(go)
    end)
end

function GameManager:StartGame()
    self.time = 20
    self.score = 0
    self.combo = 0
    local im = require('Game.Manager.InventoryManager')
    im:RandomGenerateItems()
    self.container:Generate()
    UIRoot.Instance:OnGameStart(self.time)
    self.timeCounter = LuaTimer.global:ListenRepeat(bind(self._TimeFlow, self), 1, self.time)
end

function GameManager:_TimeFlow()
    if not self.allowTimeFlow then return end
    self.time = self.time - 1
    if self.time < 1 then
        self:EngGame()
    else
        UIRoot.Instance:UpdateUI(UIConst.UIUpdateType.TimeLeft, self.time)
    end
    
end

function GameManager:EngGame()
    LuaTimer.global:Dispose(self.timeCounter)
    -- 清理元素
    Container.Instance:ClearAllElements()
    ResManager.ClearPool()
    -- 弹出结算界面
    UIRoot.Instance:OnGameEnd(self.score, self.combo)

end

function GameManager:StopTimeFlow(flag)
    print('StopTimeFlow ' .. tostring(flag))
    if flag then
        if self.timeCounter then
            LuaTimer.global:Dispose(self.timeCounter)
        end
        self.timeCounter = nil
    else
        if self.timeCounter then return end
        self.timeCounter = LuaTimer.global:ListenRepeat(bind(self._TimeFlow, self), 1, self.time)
    end
end

function GameManager:UseInventory(type)
    if type == GameConst.InventoryType.Shuffle then
        Container.Instance:DOShuffle()
    elseif type == GameConst.InventoryType.Line then
        Container.Instance:DODismissRandomLine()
    elseif type == GameConst.InventoryType.Type then
        Container.Instance:DODismissRandomType()
    end
end

function GameManager:AddCombo(combos)
    self.combo = combos
    UIRoot.Instance:UpdateUI(UIConst.UIUpdateType.Combo, self.combo)
end

function GameManager:AddScore(score)
    self.score = self.score + score
    UIRoot.Instance:UpdateUI(UIConst.UIUpdateType.Score, self.score)
end

return GameManager.New()
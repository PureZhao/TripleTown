local Class = require('Core.Class')
local ResManager = require('Game.Manager.ResManager')
local ResConst = require('Game.Const.ResConst')
local Container = require('Game.Container')
local GameConst = require('Game.Const.GameConst')
local LuaBehaviour = require('Core.LuaBehaviour')
local UIRoot = require('Game.UI.UIRoot')
local Timer = require('Game.Util.Timer')
local UIConst = require('Game.UI.UIConst')

---@class GameManager
local GameManager = Class('GameManager')

function GameManager:__init()
    self.combo = 0
    self.score = 0
    self.time = 0
    self.container = nil
    ResManager.LoadGameObject(ResConst.Container, CSE.Vector3.zero, CSE.Quaternion.identity, function (go)
        self.container = LuaBehaviour.GetLua(go)
    end)
end

function GameManager:StartGame()
    self.time = 120
    self.score = 0
    self.combo = 0
    local im = require('Game.Manager.InventoryManager')
    im:RandomGenerateItems()
    self.container:Generate()
    UIRoot.Instance:OnGameStart(self.time)
    Timer.global:ListenRepeat(function ()
        self.time = self.time - 1
        UIRoot.Instance:UpdateUI(UIConst.UIUpdateType.TimeLeft, self.time)
    end, 1, self.time)
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

return GameManager.New()
local Class = require('Core.Class')
local ResManager = require('Game.Manager.ResManager')
local ResConst = require('Game.Const.ResConst')
local Container = require('Game.Container')
local GameConst = require('Game.Const.GameConst')
local LuaBehaviour = require('Core.LuaBehaviour')

---@class GameManager
local GameManager = Class('GameManager')

function GameManager:__init()
    self.comboCount = 0
    self.score = 0

    ResManager.LoadGameObject(ResConst.Container, CSE.Vector3.zero, CSE.Quaternion.identity, function (go)
        local lua = LuaBehaviour.GetLua(go)
        lua:Generate()
    end)
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
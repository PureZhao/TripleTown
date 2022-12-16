local ResManager = require('Game.Manager.ResManager')
local ResConst = require('Game.Const.ResConst')
local CSType = require('Core.CSType')
local Coroutine = require('Core.Coroutine')
local DataConst = require('Game.Const.DataConst')
local LuaBehaviour = require('Core.LuaBehaviour')
local Timer = require('Game.Util.Timer')

local timer = Timer.New()
ResManager.LoadGameObject(ResConst.Container, CSE.Vector3.zero, CSE.Quaternion.identity, function (go)
    local lua = LuaBehaviour.GetLua(go)
    lua:Generate()
end)



-- require('Core.polyfill')
-- local Vector2 = require('Core.Vector2')
-- local v1 = Vector2.New(1, 1)
-- local v2 = Vector2.New(1, 1)
-- print(v1 == v2)
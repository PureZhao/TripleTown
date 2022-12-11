local ResManager = require('Game.Manager.ResManager')
local ResConst = require('Game.Const.ResConst')
local CSType = require('Core.CSType')

ResManager.LoadAsset(ResConst.BlueSprite, CSType.Sprite, function (obj)
    logInfo(obj.name)
end)
local Class = require('Core.Class')
local CSType = require('Core.CSType')

---@class GameManager
local GameManager = Class('GameManager')
GameManager.Instance = nil

function GameManager:__init()
    GameManager.Instance = self
end

return GameManager
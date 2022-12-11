local Class = require('Core.Class')
local CSType = require('Core.CSType')
local LuaBehaviour = require('Core.LuaBehaviour')
local ResManager = require('Game.Manager.ResManager')
local ResConst = require('Game.Const.ResConst')

---@class Element : LuaBehaviour
local Element = Class('Element', LuaBehaviour)

function Element:__Define()
    self.spriteCount = CSType.Int32
end

function Element:__init()
    self.row = 1
    self.col = 1
    self.renderer = self.transform:GetComponent(CSType.SpriteRenderer)
    
    self.type = 1
end

function Element:SetPos(row, col)
    self.row = row
    self.col = col
    local x = -4.5 + (row - 1) * 0.5
    local y = -4.5 + (col - 1) * 0.5
    self.transform.position = CSE.Vector2(x, y)
end

function Element:PlayTown()
    
end

function Element:OnMouseEnter()
    
end

function Element:OnMouseDown()
    logInfo(self.gameObject.name .. " Down")
    logInfo(self.transform.position)
end

function Element:OnMouseDrag()
    logInfo(self.gameObject.name .. " Drag")
    logInfo(self.transform.position)
end

return Element
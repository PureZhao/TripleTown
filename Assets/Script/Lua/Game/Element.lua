local Class = require('Core.Class')
local CSType = require('Core.CSType')
local LuaBehaviour = require('Core.LuaBehaviour')

local Element = Class('Element', LuaBehaviour)

function Element:__Define()
    self.sprites = CSType.ListSprite
end

function Element:__init()
    self.row = 1
    self.col = 1
    self.canTown = false
    self.renderer = self.transform:GetComponent(CSType.SpriteRenderer)
    self.spriteCount = self.sprites.Count
    self.index = 0
    self.timer = 0.5
    self.type = 1
end

function Element:SetPos(row, col)
    self.row = row
    self.col = col
    local x = -4.5 + (row - 1) * 0.5
    local y = -4.5 + (col - 1) * 0.5
    self.transform.position = CSE.Vector2(x, y)
end

function Element:Update()
    self.timer = self.timer - CSE.Time.deltaTime
    if self.timer <= 0 then
        self.renderer.sprite = self.sprites[self.index]
        self.index = self.index + 1
        if self.index >= self.spriteCount then
            self.index = 0
        end
        self.timer = 0.1
    end
end

function Element:OnMouseEnter()
    
end

function Element:OnMouseDown()
    
end

return Element
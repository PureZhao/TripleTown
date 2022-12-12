local Class = require('Core.Class')
local CSType = require('Core.CSType')
local LuaBehaviour = require('Core.LuaBehaviour')
local ResManager = require('Game.Manager.ResManager')
local ResConst = require('Game.Const.ResConst')
local DataConst = require('Game.Const.DataConst')
local Coroutine = require('Core.Corountine')
local Timer = require('Game.Util.Timer')

---@class Element : LuaBehaviour
local Element = Class('Element', LuaBehaviour)

function Element:__Define()
    self.type = CSType.ElementType
end

function Element:__init()
    self.row = 1
    self.col = 1
    self.renderer = self.transform:GetComponent(CSType.SpriteRenderer)
    self.sprites = {}
    ResManager.LoadAsset(ResConst.ElementSprites[self.type], CSType.Texture2D, function (texture)

        local rect = CSE.Rect(0, 0, 100, 100)
        local pivot = CSE.Vector2(0.5, 0.5)
        local sprites = {}
        for k, v in pairs(DataConst.ElementSpriteSplitPos) do
            rect.position = CSE.Vector2(v.x, v.y)
            sprites[k] = CSE.Sprite.Create(texture, rect, pivot)
        end
        self.sprites = sprites
        self.renderer.sprite = sprites[1]
    end)
    self.timer = Timer.New()
    self.timer:Delay(5, bind(self.PlayTown, self))
end

function Element:SetPos(row, col)
    self.row = row
    self.col = col
    local x = -4.5 + (row - 1) * 0.5
    local y = -4.5 + (col - 1) * 0.5
    self.transform.position = CSE.Vector2(x, y)
end

function Element:PlayTown()
    local routine = Coroutine.Create(function ()
        for _, v in pairs(self.sprites) do
            self.renderer.sprite = v
            coroutine.yield(CSE.WaitForSeconds(1))
        end
        self.renderer.sprite = 
    end)
    self.host:StartCoroutine(routine)
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
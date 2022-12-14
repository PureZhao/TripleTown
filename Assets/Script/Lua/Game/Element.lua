local Class = require('Core.Class')
local CSType = require('Core.CSType')
local LuaBehaviour = require('Core.LuaBehaviour')
local ResManager = require('Game.Manager.ResManager')
local ResConst = require('Game.Const.ResConst')
local DataConst = require('Game.Const.DataConst')
local Coroutine = require('Core.Coroutine')
local Container = require('Game.Container')

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
end

function Element:SetPos(row, col)
    -- x -4.5 ~ 2.5
    -- y -4.5 ~ 2.5
    self.row = row
    self.col = col
    local x = -4.5 + (col - 1)
    local y = -4.5 + (row - 1)
    self.transform.position = CSE.Vector3(x, y, 0)
end

function Element:PlayTown()
    logInfo("Play Town" .. self.row .. ' ' .. self.col)
    local routine = Coroutine.Create(bind(self._TownCoroutine, self))
    self.host:StartCoroutine(routine)
end

function Element:BeOnSeleted(isSelected)
    if isSelected then
        self.renderer.sprite = self.sprites[2]
    else
        self.renderer.sprite = self.sprites[1]
    end
end

function Element:_TownCoroutine()
    for _, v in pairs(self.sprites) do
        self.renderer.sprite = v
        coroutine.yield(CSE.WaitForSeconds(0.1))
    end
    self.renderer.sprite = nil
    Container.Instance:TownCountMinus()
    ResManager.FreeObject(self.gameObject)
end

function Element:OnMouseDown()
    logInfo(self.gameObject.name .. " " .. tostring(self.row ) .. " " .. tostring(self.col))
    self:BeOnSeleted(true)
    Container.Instance:AddToSelect(self.row, self.col)
end

---@param state boolean
function Element:EnableMouseClick(state)
    self.host:ActivateMouseEvent(state)
end

return Element
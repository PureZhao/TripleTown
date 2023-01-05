local Class = require('Core.Class')
local CSType = require('Core.CSType')
local LuaBehaviour = require('Core.LuaBehaviour')
local ResManager = require('Game.Manager.ResManager')
local ResConst = require('Game.Const.ResConst')
local DataConst = require('Game.Const.DataConst')
local Coroutine = require('Core.Coroutine')
local Container = require('Game.Container')
local LuaTimer = require('Game.Util.LuaTimer')

---@class Element : LuaBehaviour
local Element = Class('Element', LuaBehaviour)

function Element:__Define()
    self.type = CSType.ElementType
end

function Element:__init()
    self.row = -1
    self.col = -1
    self.renderer = self.transform:GetComponent(CSType.SpriteRenderer)
    self.sprites = {}
    self.spriteCount = 0
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
        self.spriteCount = table.len(self.sprites)
    end)
end

function Element:SetPos(row, col)
    -- 如果位置没变就不动
    if row == self.row and col == self.col then return end
    -- x -4.5 ~ 2.5
    -- y -4.5 ~ 2.5
    self.row = row
    self.col = col
    local x = -4.5 + (col - 1)
    local y = -4.5 + (row - 1)
    local pos = CSE.Vector3(x, y, 0)
    local routine = Coroutine.Create(bind(self._MoveCoroutine, self), pos)
    self.host:StartCoroutine(routine)
end

function Element:BeOnSeleted(isSelected)
    if isSelected then
        self.renderer.sprite = self.sprites[2]
    else
        self.renderer.sprite = self.sprites[1]
    end
end

function Element:PlayTownAnimation()
    local cur = 1
    LuaTimer.global:ListenRepeat(function ()
        if cur <= self.spriteCount then
            self.renderer.sprite = self.sprites[cur]
        else
            self.renderer.sprite = nil
            Container.Instance:TweenCountMinus()
            ResManager.FreeObject(self.gameObject)
        end
        cur = cur + 1
    end, 0.1, self.spriteCount + 1)
end

function Element:_MoveCoroutine(pos)
    self.transform:DOMove(pos, 1)
    coroutine.yield(CSE.WaitForSeconds(1.1))
    Container.Instance:TweenCountMinus()
end

function Element:OnMouseDown()
    self:BeOnSeleted(true)
    Container.Instance:DOJudge(self.row, self.col)
end

---@param state boolean
function Element:EnableMouseClick(state)
    self.host:ActivateMouseEvent(state)
end

return Element
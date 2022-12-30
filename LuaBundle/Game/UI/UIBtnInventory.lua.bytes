local Class = require('Core.Class')
local CSType = require('Core.CSType')
local LuaBehaviour = require('Core.LuaBehaviour')
local GameConst = require('Game.Const.GameConst')
local ResConst  = require('Game.Const.ResConst')
local ResManager= require('Game.Manager.ResManager')
local Timer = require('Game.Util.Timer')
local UIRoot = require('Game.UI.UIRoot')
local UIConst = require('Game.UI.UIConst')

---@class UIBtnInventory : LuaBehaviour
local UIBtnInventory = Class("UIBtnInventory", LuaBehaviour)

function UIBtnInventory:__Define()
    self.inventoryRoot = CSType.RectTransform
end

function UIBtnInventory:__init()
    -- self:_InitItems()
    self.button = self.transform:GetComponent(CSType.Button)
    -- 注册OnClick函数
    self.button.onClick:AddListener(bind(self.OnClick, self))
    UIRoot.Instance:Registry(bind(self._InitItems, self))
    self.itemsGenerated = false
end

function UIBtnInventory:_InitItems(type, ...)
    if type ~= UIConst.UIUpdateType.InitItems or self.itemsGenerated then return end
    self.itemsGenerated = true
    self.inventoryRoot.gameObject:SetActive(false)
    local inventories = require('Game.Manager.InventoryManager').inventories
    ResManager.LoadGameObject(ResConst.BtnItem, nil, nil, function (go)
        go:SetParent(self.inventoryRoot)
        ---@type UIBtnItem
        local lua = LuaBehaviour.GetLua(go)
        lua:SetItem(inventories[GameConst.InventoryType.Shuffle])
    end)

    ResManager.LoadGameObject(ResConst.BtnItem, nil, nil, function (go)
        go:SetParent(self.inventoryRoot)
        ---@type UIBtnItem
        local lua = LuaBehaviour.GetLua(go)
        lua:SetItem(inventories[GameConst.InventoryType.Line])
    end)

    ResManager.LoadGameObject(ResConst.BtnItem, nil, nil, function (go)
        go:SetParent(self.inventoryRoot)
        ---@type UIBtnItem
        local lua = LuaBehaviour.GetLua(go)
        lua:SetItem(inventories[GameConst.InventoryType.Type])
    end)

end

function UIBtnInventory:OnClick()
    self.button.interactable = false
    self.inventoryRoot.position = CSE.Input.mousePosition
    self.inventoryRoot.localScale = CSE.Vector3.zero
    self.inventoryRoot.gameObject:SetActive(true)
    self.inventoryRoot:DOScale(1, 0.5)
    self.handler = Timer.global:ListenUpdate(bind(self._OnMouseClickOut, self))
end

function UIBtnInventory:_OnMouseClickOut()
    local rect = self.inventoryRoot.rect
    local position = self.inventoryRoot.position
    local xmin = position.x - rect.width / 2
    local xmax = position.x + rect.width / 2
    local ymin = position.y - rect.height / 2
    local ymax = position.y + rect.height / 2
    if CSE.Input.GetMouseButtonDown(0) then
        local pos = CSE.Input.mousePosition
        if pos.x < xmin or pos.x > xmax or pos.y < ymin or pos.y > ymax then
            self.inventoryRoot:DOScale(0, 0.5):OnComplete(function ()
                self.inventoryRoot.gameObject:SetActive(false)
                self.button.interactable = true
                Timer.global:Dispose(self.handler)
            end)
        end
    end
end

return UIBtnInventory
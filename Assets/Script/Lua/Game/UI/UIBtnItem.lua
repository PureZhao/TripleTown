local Class = require('Core.Class')
local CSType = require('Core.CSType')
local LuaBehaviour = require('Core.LuaBehaviour')
local UIRoot = require('Game.UI.UIRoot')
local UIConst = require('Game.UI.UIConst')
local LuaTimer = require('Game.Util.LuaTimer')

---@class UIBtnItem : LuaBehaviour
local UIBtnItem = Class("UIBtnItem", LuaBehaviour)

function UIBtnItem:__Define()
    self.nameText = CSType.Text
    self.countText = CSType.Text
end

function UIBtnItem:__init()
    self.useGap = 1.5
    self.canUse = true
    self.button = self.transform:GetComponent(CSType.Button)
    self.type = UIConst.UIUpdateType.Inventory
    self.inventoryType = nil
    -- 注册OnClick函数
    self.button.onClick:AddListener(bind(self.OnClick, self))
    -- 注册到UIRoot去
    UIRoot.Instance:Registry(bind(self._UpdateUI, self))
end

function UIBtnItem:SetItem(inventory)
    self.countText.text = tostring(inventory.count)
    self.nameText.text = inventory.name
    self.inventoryType = inventory.type
end

function UIBtnItem:_UpdateUI(type, ...)
    if type ~= self.type then
        return
    end
    local param = {...}
    local inventory = param[1]
    if inventory.type ~= self.inventoryType then return end
    self:SetItem(inventory)
end

function UIBtnItem:OnClick()
    if not self.canUse then
        return
    end
    self.canUse = false
    LuaTimer.global:Delay(self.useGap, function ()
        self.canUse = true
    end)
    local InventoryManager = require('Game.Manager.InventoryManager')
    InventoryManager:UseItem(self.inventoryType, 1)
end

return UIBtnItem
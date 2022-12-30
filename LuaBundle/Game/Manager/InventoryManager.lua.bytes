local Class = require('Core.Class')
local GameConst = require('Game.Const.GameConst')
local Inventory = require('Game.Inventory')
local UIRoot = require('Game.UI.UIRoot')
local UIConst= require('Game.UI.UIConst')
local Timer = require('Game.Util.Timer')
require('Core.polyfill')

---@class InventoryManager
local InventoryManager = Class('InventoryManager')

function InventoryManager:__init()
    self.inventoryInfos = self:_InitInventoryInfo()
    ---@type table<GameConst.InventoryType, Inventory>
    self.inventories = {}
end

function InventoryManager:_InitInventoryInfo()
    -- 拿信息
    -- 测试占位
    return {
        [GameConst.InventoryType.Shuffle] = {type = GameConst.InventoryType.Shuffle, name = "Shuffle"},
        [GameConst.InventoryType.Line] = {type = GameConst.InventoryType.Line, name = "Line"},
        [GameConst.InventoryType.Type] = {type = GameConst.InventoryType.Type, name = "Type"},
    }
end

function InventoryManager:RandomGenerateItems()
    math.randomseed(os.time())
    for i = 1, 10 do
        local type = table.randomInPair(GameConst.InventoryType)
        self:ObtainItem(type, 1)
    end
end

function InventoryManager:ObtainItem(type, count)
    if not self.inventories[type] then
        self.inventories[type] = Inventory.New(self.inventoryInfos[type])
    end
    local inventory = self.inventories[type]
    if inventory:Obtain(count) then
        UIRoot.Instance:UpdateUI(UIConst.UIUpdateType.Inventory, inventory)
    end
end

function InventoryManager:UseItem(type, count)
    local inventory = self.inventories[type]
    if inventory:Use(count) then
        UIRoot.Instance:UpdateUI(UIConst.UIUpdateType.Inventory, inventory)
        require('Game.Manager.GameManager'):UseInventory(inventory.type)
    end
end

function InventoryManager:Up()
    
end

function InventoryManager:__delete()
    
end

return InventoryManager.New()
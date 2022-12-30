local Class = require('Core.Class')

---@class Inventory
local Inventory = Class('Inventory')

function Inventory:__init(info)
    self.type = info.type
    self.name = info.name
    self.count = 0
    self.maxCount = 999
end

function Inventory:Obtain(count)
    if self.count + count > self.maxCount then
        return false
    end
    self.count = self.count + count
    return true
end

function Inventory:Use(count)
    if self.count - count < 0 then
        return false
    end
    self.count = self.count - count
    return true
end

function Inventory:__delete()
    
end



return Inventory
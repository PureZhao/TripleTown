local Class = require("Core.Class")

---@class EventListener
local EventListener = Class("EventListener")

local Type = {
    Once = "Once",
    Normal = "Normal",
}

function EventListener:__init()
    self.listeners = {
        [Type.Normal] = {},
        [Type.Once] = {},
    }
end

function EventListener:__delete()
    
end

function EventListener:_CreateHandler()
    
end

function EventListener:_AddEvent(type, func)
    
end

function EventListener:ListenOnce()
    
end

function EventListener:Listen()
    
end

function EventListener:Emit()
    
end

function EventListener:Dispose(handle)
    
end

return EventListener
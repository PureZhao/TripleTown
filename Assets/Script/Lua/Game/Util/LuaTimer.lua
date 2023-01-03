local Class = require("Core.Class")
local List = require('Core.Collections.List')
local Scheduler = CS.GameCore.Scheduler.Instance

---@class LuaTimer
local LuaTimer = Class("LuaTimer")

local Type = {
    Delay = 1,
    DelayFrames = 2,
    Update = 3,
    LateUpdate = 4,
    Repeat = 5,
}

function LuaTimer:__init()
    ---@type table<Type, List>
    self.dict = {
        [Type.Delay] = List.New(),
        [Type.DelayFrames] = List.New(),
        [Type.Update] = List.New(),
        [Type.LateUpdate] = List.New(),
        [Type.Repeat] = List.New(),
    }
    Scheduler:ListenUpdate(bind(self._Update, self))
    Scheduler:ListenLateUpdate(bind(self._LateUpdate, self))
end

function LuaTimer:_PushIntoDict(node, type)
    ---@type List
    local targetList = self.dict[type]
    if targetList then
        return targetList:Add(node)
    end
end

function LuaTimer:Delay(time, func)
    if not func then
        return
    end
    local callTime = CSE.Time.realtimeSinceStartup + time
    local node = {func = func, callTime = callTime, deprecated = false}
    return self:_PushIntoDict(node, Type.Delay)
end

function LuaTimer:DelayFrames(frames, func)
    if not func then
        return
    end
    local node = {func = func, leftFrames = frames, deprecated = false}
    return self:_PushIntoDict(node, Type.DelayFrames)
end

function LuaTimer:ListenUpdate(func)
    if not func then
        return
    end
    local node = {func = func, deprecated = false}
    return self:_PushIntoDict(node, Type.Update)
end

function LuaTimer:ListenLateUpdate(func)
    if not func then
        return
    end
    local node = {func = func, deprecated = false}
    return self:_PushIntoDict(node, Type.LateUpdate)
end

function LuaTimer:ListenRepeat(func, interval, times)
    if not func then
        return
    end
    local callTime = CSE.Time.realtimeSinceStartup + interval
    local node = {func = func, interval = interval, times = times, callTime = callTime}
    return self:_PushIntoDict(node, Type.Repeat)
end

function LuaTimer:Dispose(node)
    if not node then
        return
    end
    node.deprecated = true
end

function LuaTimer:_Update()
    local now = CSE.Time.realtimeSinceStartup
    -- delay
    local targetList = self.dict[Type.Delay]
    for i, node in targetList:Pairs() do
        local val = node.val
        if not val.deprecated and now >= val.callTime then
            val.deprecated = true
            val.func()
        end
    end
end

function LuaTimer:_LateUpdate()
    
end

function LuaTimer:_CleanDeprecated()
    
end

---@param funcList List
---@param type Type
function LuaTimer:_IterCall(funcList, type)
    local now = CSE.Time.realtimeSinceStartup
    for i, node in funcList:Pairs() do
        local val = node.val
        if not val.deprecated then
            local func = val.func
            if type == Type.Delay and now >= val.callTime then
                
            elseif type == Type.DelayFrames then
            elseif type == Type.LateUpdate then
            elseif type == Type.Update then
            elseif type == Type.Repeat then
            end
        end
    end
end

---@type LuaTimer
LuaTimer.global = LuaTimer.New()

return LuaTimer
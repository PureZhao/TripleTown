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
    local node = {func = func, interval = interval, times = times, callTime = callTime, deprecated = false}
    return self:_PushIntoDict(node, Type.Repeat)
end

function LuaTimer:Dispose(node)
    if not node then
        return
    end
    node.value.deprecated = true
end

function LuaTimer:_Update()
    local now = CSE.Time.realtimeSinceStartup
    for type, list in pairs(self.dict) do
        if type ~= Type.LateUpdate then
            self:_IterCall(list, type, now)
        end
    end
end

function LuaTimer:_LateUpdate()
    local now = CSE.Time.realtimeSinceStartup
    local list = self.dict[Type.LateUpdate]
    self:_IterCall(list, Type.LateUpdate, now)
    self:_CleanDeprecated()
end

function LuaTimer:_CleanDeprecated()
    local dict = {}
    for type, list in pairs(self.dict) do
        local t = List.New()
        for i, val in list:Pairs() do
            if val and not val.deprecated then
                t:Add(val)
            end
        end
        dict[type] = t
    end
    self.dict = dict
end

---@param funcList List
---@param type Type
function LuaTimer:_IterCall(funcList, type, now)
    for i, val in funcList:Pairs() do
        if val and not val.deprecated then
            local func = val.func
            if type == Type.Delay and now >= val.callTime then
                val.deprecated = true
                func()
            elseif type == Type.DelayFrames then
                if val.leftFrames > 0 then
                    val.leftFrames = val.leftFrames - 1
                else
                    val.deprecated = true
                    func()
                end
            elseif type == Type.LateUpdate or type == Type.Update then
                func()
            elseif type == Type.Repeat then
                if val.times > 0 and now >= val.callTime then
                    val.times = val.times - 1
                    val.callTime = val.callTime + val.interval
                    func()
                elseif val.times <= 0 then
                    val.deprecated = true
                end
            end
        end
    end
end



---@type LuaTimer
LuaTimer.global = LuaTimer.New()

return LuaTimer
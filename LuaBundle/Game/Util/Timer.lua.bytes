local Class = require("Core.Class")
local Scheduler = CS.GameCore.Scheduler.Instance

---@class Timer
local Timer = Class("Timer")

function Timer:__init()
    
end

function Timer:Delay(time, func)
    if not func then
        return
    end
    return Scheduler:Delay(time, function ()
        func()
    end)
end

function Timer:DelayFrame(func)
    if not func then
        return
    end
    return Scheduler:DelayFrame(function ()
        func()
    end)
end

function Timer:DelayFrames(frames, func)
    if not func then
        return
    end
    return Scheduler:DelayFrames(frames, function ()
        func()
    end)
end

function Timer:ListenUpdate(func)
    if not func then
        return
    end
    return Scheduler:ListenUpdate(function ()
        func()
    end)
end

function Timer:ListenLateUpdate(func)
    if not func then
        return
    end
    return Scheduler:ListenLateUpdate(function ()
        func()
    end)
end

function Timer:ListenRepeat(func, interval, times)
    if not func then
        return
    end
    return Scheduler:ListenRepeat(interval, times, function ()
        func()
    end)
end

function Timer:Dispose(identity)
    if not identity then
        return
    end
    Scheduler:DisposeListener(identity)
end

---@type Timer
Timer.global = Timer.New()

return Timer
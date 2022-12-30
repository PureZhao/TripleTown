---@class Coroutine
local Coroutine = {}

local move_end = {}
-- 协程元表
-- 主要是实现MoveNext和Reset函数
local co_mt = {
    __index = {
        MoveNext = function(self)
            self.Current = self.co()
            if self.Current == move_end then
                self.Current = nil
                return false
            else
                return true
            end
        end;
        Reset = function(self)
            self.co = coroutine.wrap(self.func)
        end
    }
}

---@return CS.System.IEnumerator
function Coroutine.Create(ienumerator, ...)
    local params = {...}
    local routine = setmetatable({
        func = function ()
            ienumerator(table.unpack(params))
            return move_end  -- 协程执行完毕返回空表
        end
    }, co_mt)
    routine:Reset()
    return routine
end

return Coroutine
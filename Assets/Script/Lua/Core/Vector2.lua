---@class Vector2
local Vector2 = {}

local mt = {
    __eq = function (v1, v2)  -- 重载 ==
        return v1.x == v2.x and v1.y == v2.y
    end
}

function Vector2.New(x, y)
    local v = {}
    setmetatable(v, mt)
    x = x or 0
    y = y or 0
    v.x = x
    v.y = y
    return v
end

return Vector2
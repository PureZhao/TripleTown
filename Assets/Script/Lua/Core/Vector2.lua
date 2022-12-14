local Vector2 = {}

function Vector2.New(x, y)
    local v = {}
    x = x or 0
    y = y or 0
    v.x = x
    v.y = y
    return v
end

return Vector2
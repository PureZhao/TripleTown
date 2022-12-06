local Vector3 = {}

function Vector3.New(x, y, z)
    return {x = x or 0, y = y or 0, z = z or 0}
end

function Vector3.Dot(va, vb)
    return va.x * vb.x + va.y * vb.y + va.z * vb.z;
end

function Vector3.Cross(va, vb)
    return Vector3.New(va.y * vb.z - va.z * vb.y, va.z * vb.x - va.x * vb.z, va.x * vb.y - va.y * vb.x)
end

function Vector3.Distance(va, vb)
    return math.sqrt((va.x - vb.x) * (va.x - vb.x) + (va.y - vb.y) * (va.y - vb.y) +(va.z - vb.z) * (va.z - vb.z))
end

function Vector3.SqrMagnitude(v)
    return v.x * v.x + v.y * v.y + v.z * v.z
end

function Vector3.Magnitude(v)
    return math.sqrt(v.x * v.x + v.y * v.y + v.z * v.z)
end

function Vector3.Normalize(v)
    local magnitude = Vector3.Magnitude(v)
    if(magnitude <= 1e-5) then
        return Vector3.zero
    end
    return Vector3.New(v.x / magnitude, v.y / magnitude, v.z / magnitude)
end

Vector3.zero = Vector3.New(0, 0, 0)
Vector3.one = Vector3.New(1, 1, 1)
Vector3.up = Vector3.New(0, 1, 0)
Vector3.down = Vector3.New(0, -1, 0)
Vector3.left = Vector3.New(-1, 0, 0)
Vector3.right = Vector3.New(1, 0, 0)
Vector3.forward = Vector3.New(0, 0, 1)
Vector3.back = Vector3.New(0, 0, -1)

return Vector3
---@param text string
---@param pattern string
---@param ignoreCase? boolean
---@return boolean
function string.contains(text, pattern, ignoreCase)
    if ignoreCase then
        text = string.lower(text)
        pattern = string.lower(pattern)
    end
    return string.find(text, pattern) ~= nil
end

---@param s string
---@return boolean
function string.empty(s)
    return s == nil or s == ""
end

function string.findAll(s, p)
    --- 匹配串为空 模式串也为空 返回为空
    if string.empty(s) or string.empty(p) then
        return nil
    end
    local indices = {}
    local len = string.len(p)
    local index = string.find(s, p)
    --- 找不到了就停止
    while index ~= nil do
        local realIndex
        if #indices >= 1 then
            --- 有上一个位置就相加以下, 这是实际位置
            realIndex = index + indices[#indices]
        else
            --- 没有就不加
            realIndex = index
        end
        table.insert(indices, realIndex)
        --- 下一个字串开始位置
        local terminal = index + len
        --- 获取下一个字串
        s = string.sub(s, terminal)
        index = string.find(s, p)
    end

    if indices[1] then
        return indices
    else
        return nil
    end

end

function string.split(s, p)
    local indices = string.findAll(s, p)
    if indices then
        local len = string.len(p)
        local slices = {}
        for i, _ in pairs(indices) do
            --- 夹逼取子串
            if indices[i - 1] then
                table.insert(slices, string.sub(s, indices[i - 1] + len, indices[i] - 1))
            else
                table.insert(slices, string.sub(s, 1, indices[i] - 1))
            end
            --- 最后一个
            if not indices[i + 1] then
                table.insert(slices, string.sub(s, indices[i] + len, string.len(s)))
            end
        end
        return slices
    else
        return nil
    end
end

function string.trim(s)
    if string.empty(s) then
        return s
    end

    local len = string.len(s)
    local head = 1
    local tail = len
    for i = 1, len do
        local c = string.sub(s, i, i)
        head = i
        if c ~= " " then
            break
        end
    end
    for i = len, 1, -1 do
        local c = string.sub(s, i, i)
        tail = i
        if c ~= " " then
            break
        end
    end
    return string.sub(s, head, tail)
end

function string.trimend(s)
    if string.empty(s) then
        return s
    end

    local len = string.len(s)
    local tail = len
    for i = len, 1, -1 do
        local c = string.sub(s, i, i)
        tail = i
        if c ~= " " then
            break
        end
    end
    return string.sub(s, 1, tail)
end


function string.trimstart(s)
    if string.empty(s) then
        return s
    end

    local len = string.len(s)
    local head = 1
    for i = 1, len do
        local c = string.sub(s, i, i)
        head = i
        if c ~= " " then
            break
        end
    end
    return string.sub(s, head, len)
end

function string.toboolean(s)
    local t = string.trim(s):lower()
    if t == "true" then
        return true
    elseif t == "false" then
        return false
    else
        assert(false, string.format("%s is not a boolean value", s))
    end
end

---@param t table
---@return any, number
function table.randomIn(t)
    if not t or #t < 1 then
        return nil, 0
    end
    local len = #t
    local index = math.random(len)
    return t[index], index
end

---@param t table
---@return any
function table.randomRemove(t)
    math.randomseed(os.time())
    if not t or #t < 1 then
        return nil
    end
    local len = #t
    local index = math.random(len)
    return table.remove(t, index)
end

---@param t table
function table.clone(t)
    if not t then
        return nil
    end
    local r = {}
    for k, v in pairs(t) do
        if type(v) == "table" then
            r[k] = table.clone(v)
        else
            r[k] = v
        end
    end
    return r
end

---@param t table
function table.clear(t)
    if t then
        for k, _ in pairs(t) do
            t[k] = nil
        end
        -- t = {}
    end
end

---@param t table
---@return number
function table.len(t)
    local len = 0
    if t then
        for k, v in pairs(t) do
            len = len + 1
        end
    end
    return len
end

---@param t table
---@param value any
---@return boolean
function table.exist(t, value)
    if t then
        for _, v in pairs(t) do
            if v == value then
                return true
            end
        end
    end
    return false
end

---@param t table
---@param value any
---@return any key if not exist, return nil
function table.index(t, value)
    if t then
        for k, v in pairs(t) do
            if v == value then
                return k
            end
        end
    end
    return nil
end

---@param t1 table
---@param t2 table
---@return table
function table.merge(t1, t2)
    if not t1 then
        return t2
    end
    if not t2 then
        return t1
    end
    if not t1 and not t2 then
        return {}
    end
    local merge = {}
    for _, v in pairs(t1) do
        table.insert(merge, v)
    end
    for _, v in pairs(t2) do
        table.insert(merge, v)
    end
    return merge
end

---@param t table
---@return table
function table.shuffle(t)
    math.randomseed(os.time())
    local arr = {}
    local cache = {}
    local count = 0
    local res ={}
    for k, v in pairs(t) do
        count = count + 1
        arr[count] = count
        cache[count] = {key = k, value = v}
    end
    -- 洗牌
    for i = 1, count do
        local j = math.random(i, count)
        arr[i], arr[j] = arr[j], arr[i]
    end
    for _, v in pairs(arr) do
        table.insert(res, cache[v].value)
    end
    return res
end
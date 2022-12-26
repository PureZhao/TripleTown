local Class = require('Core.Class')

---@class List
local List = Class('List')

function List:__init()
    self.head = nil
    self.tail = nil
    self.length = 0
    self._lengthUpdate = false
end

function List:Add(val)
    self._lengthUpdate = false
    if self.head then
        local node = {prev = self.tail, value = val, next = nil}
        self.tail.next = node
        self.tail = node
    else
        self.head = {prev = nil, value = val, next = nil}
        self.tail = self.head
    end
end

function List:Remove(val)
    self._lengthUpdate = false
    local move = self.head
    -- 如果是头
    if move.value == val then
        -- 如果头尾相等(只有这么一个节点)
        if self.head == self.tail then
            self.head = nil
            self.tail = nil
        else
            move = move.next
            move.prev = nil
            self.head = move
        end
    else
        while move do
            if move.value == val then
                break
            end
            move = move.next
        end
        -- 如果是尾巴
        if move == self.tail then
            move = move.prev
            move.next = nil
            self.tail = move
        else
            -- 如果不是尾巴(在队列中间)
            local prev = move.prev
            local next = move.next
            prev.next = next
            next.prev = prev
        end
    end
end

function List:Length()
    if self._lengthUpdate then
        return self.length
    else
        for _, v in self:Pairs() do end
        return self.length
    end
end

function List:__delete()
    
end

function List:Pairs()
    local move = self.head
    local i = 0
    local r
    return function ()
        if move then
            r = move.value
            move = move.next
            i = i + 1
        else
            self.length = i
            self._lengthUpdate = true
            i = nil
            r = nil
        end
        return i, r
    end
end

-- local function Iter(const, i)
--     if i then
--         local v = i.value
--         return i.next, v
--     else
--         return nil, nil
--     end
-- end

-- function List:PPairs()
--     return Iter, self, self.head
-- end

return List
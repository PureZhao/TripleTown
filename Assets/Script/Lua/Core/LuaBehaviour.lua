local Class = require("Core.Class")
local CSType = require("Core.CSType")

---@class LuaBehaviour
local LuaBehaviour = Class("LuaBehaviour")

local function CallDefine(classType, defineSelf)
    if classType.super then
        CallDefine(classType.super, defineSelf)
    end

    if classType.__Define then
        classType.__Define(defineSelf)
    end
end

local defineSelf = {}
local defineStore = {}
-- 保证顺序，需要按照顺序往表里插，保证结果是正确的
setmetatable(defineSelf, {__newindex = function(_ , varName, varType)
    table.insert(defineStore, {name = varName, value = varType})
end})

local function _ClearLocalDefineList()
    for i = 1, #defineStore do
        defineStore[i] = nil
    end
end

function LuaBehaviour:Define()
    --这个 define 可能会多次被调用。 只执行一次即可
    if rawget(self, "_DefineList") ~= nil then return end
    _ClearLocalDefineList()
    self._DefineList = {}

    CallDefine(self, defineSelf)
    for key, value in ipairs(defineStore) do
        table.insert(self._DefineList, { name = value.name, type = value.value})
    end
end


function LuaBehaviour.GetLua(obj)

end

return LuaBehaviour
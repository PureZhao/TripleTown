local IsNull = require('Core.IsNull')

local UnityUtils = {}

function UnityUtils.GetComponent(objOrTrans, type)
    local comp = objOrTrans:GetComponent(type)
    if not IsNull(comp) then
        return comp
    end
    return nil
end

return UnityUtils
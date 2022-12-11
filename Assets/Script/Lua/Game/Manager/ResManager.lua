local AssetsManager = CS.GameCore.AssetsManager.Instance

---@class ResManager
local ResManager = {}

---@param assetPath string
---@param type CSType
---@param onSuccess function
function ResManager.LoadAsset(assetPath, type, onSuccess)
    AssetsManager:LoadAsset(assetPath, type,
    ---@param obj UnityEngine.Object
    function (obj)
        logInfo(obj)
        if onSuccess then
            onSuccess(obj)
        end
    end)
end


function ResManager.LoadGameObject(assetPath, position, rotation, onSuccess)
    position = position or CSE.Vector3.zero
    rotation = rotation or CSE.Quaternion.identity
    AssetsManager:LoadGameObject(assetPath, 
    ---@param go UnityEngine.GameObject
    function (go)
        go.transform.position = position
        go.transform.rotation = rotation
        if onSuccess then
            onSuccess(go)
        end
    end)
end

---@param UnityEngine.Object
function ResManager.FreeObject(obj)
    AssetsManager:FreeObject(obj)
end

return ResManager
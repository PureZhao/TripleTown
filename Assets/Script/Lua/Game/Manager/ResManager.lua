local AssetsManager = CS.GameCore.AssetsManager.Instance

local ResManager = {}


function ResManager.LoadAsset(assetPath, onSuccess)
    AssetsManager:LoadAsset(assetPath, 
    ---@param obj UnityEngine.Object
    function (obj)
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
return ResManager
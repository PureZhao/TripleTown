local AssetsManager = CS.GameCore.AssetsManager.Instance

local ResManager = {}


function ResManager.LoadObject(assetPath, onSuccess)
    AssetsManager:LoadAsset(assetPath, onSuccess)
end

return ResManager
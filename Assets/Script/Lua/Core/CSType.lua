local CSE = CS.UnityEngine
local CSO = CS.System
local CSG = CS.System.Collections.Generic

---@class CSType
local CSType = {}
setmetatable(CSType, {
    __index = function (t, k)
        if k == nil then
            print("<color=red>Error Type</color>", t, k)
        end
        t[k] = typeof(k)
        return rawget(t, k)
    end
})

local function List(type)
    return CSG.List(type)
end

CSType.Object = CSType[CSE.Object]
CSType.GameObject = CSType[CSE.GameObject]
CSType.Transform = CSType[CSE.Transform]
CSType.Collider = CSType[CSE.Collider]
CSType.BoxCollider = CSType[CSE.BoxCollider]
CSType.CapsuleCollider = CSType[CSE.CapsuleCollider]
CSType.SphereCollider = CSType[CSE.SphereCollider]
CSType.MeshCollider = CSType[CSE.MeshCollider]
CSType.Rigidbody = CSType[CSE.Rigidbody]
CSType.AudioListener = CSType[CSE.AudioListener]
CSType.AudioSource = CSType[CSE.AudioSource]
CSType.AudioClip = CSType[CSE.AudioClip]
CSType.Animation = CSType[CSE.Animation]
CSType.Animator = CSType[CSE.Animator]
CSType.Camera = CSType[CSE.Camera]
CSType.Renderer = CSType[CSE.Renderer]
CSType.MeshRenderer = CSType[CSE.MeshRenderer]
CSType.MeshFilter = CSType[CSE.MeshFilter]
CSType.ParticleSystem = CSType[CSE.ParticleSystem]
CSType.Image = CSType[CSE.UI.Image]
CSType.Text = CSType[CSE.UI.Text]
CSType.String = CSType[CSO.String]
CSType.Int32 = CSType[CSO.Int32]
CSType.Int64 = CSType[CSO.Int64]
CSType.Vector2 = CSType[CSE.Vector2]
CSType.Vector3 = CSType[CSE.Vector3]
CSType.Vector4 = CSType[CSE.Vector4]
CSType.ListInt32 = CSType[List(CSType.Int32)]

CSType.LuaBehaviour = CSType[CS.GameCore.LuaBehaviour]

return CSType
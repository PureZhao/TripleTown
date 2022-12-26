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
CSType.AudioClip = CSType[CSE.AudioClip]
CSType.Camera = CSType[CSE.Camera]

CSType.RectTransform = CSType[CSE.RectTransform]
CSType.String = CSType[CSO.String]
CSType.Int32 = CSType[CSO.Int32]
CSType.Vector2 = CSType[CSE.Vector2]
CSType.Vector3 = CSType[CSE.Vector3]
CSType.Vector4 = CSType[CSE.Vector4]

CSType.Sprite = CSType[CSE.Sprite]
CSType.Texture2D = CSType[CSE.Texture2D]
CSType.SpriteRenderer = CSType[CSE.SpriteRenderer]

CSType.LuaBehaviour = CSType[CS.GameCore.LuaBehaviour]
CSType.ElementType = CSType[CS.GameCore.ElementType]
CSType.ToolType = CSType[CS.GameCore.ToolType]
-- Collections
CSType.ListInt32 = CSType[List(CSType.Int32)]
CSType.ListSprite = CSType[List(CSType.Sprite)]

CSType.Button = CSType[CSE.UI.Button]
CSType.Image = CSType[CSE.UI.Image]
CSType.Text = CSType[CSE.UI.Text]

return CSType
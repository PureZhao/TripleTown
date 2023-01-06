local ResConst = {}

local AssetsDir = "Assets/Res/"

local ElementDir = AssetsDir .. "Elements/"
local BlueElement = ElementDir .. "Blue/BlueElement.prefab"
local RedElement = ElementDir .. "Red/RedElement.prefab"
local YellowElement = ElementDir .. "Yellow/YellowElement.prefab"
local BrownElement = ElementDir .. "Brown/BrownElement.prefab"
local PurpleElement = ElementDir .. "Purple/PurpleElement.prefab"

ResConst.Elements = {
    BlueElement,
    RedElement,
    YellowElement,
    BrownElement,
    PurpleElement,
}


ResConst.ElementSprites = {
    [CS.GameCore.ElementType.Blue] = "Assets/Res/Elements/Blue/Blue.png",
    [CS.GameCore.ElementType.Brown] = "Assets/Res/Elements/Brown/Brown.png",
    [CS.GameCore.ElementType.Purple] = "Assets/Res/Elements/Purple/Purple.png",
    [CS.GameCore.ElementType.Red] = "Assets/Res/Elements/Red/Red.png",
    [CS.GameCore.ElementType.Yellow] = "Assets/Res/Elements/Yellow/Yellow.png",
}

ResConst.Container = AssetsDir .. "Container.prefab"
ResConst.UIRoot = AssetsDir .. "UI/UIRoot.prefab"
ResConst.BtnItem = AssetsDir .. "UI/BtnItem.prefab"
ResConst.PanelResult = AssetsDir .. "UI/Result.prefab"

return ResConst
local ResConst = {}

local AssetsDir = "Assets/Res/"

local ElementDir = AssetsDir .. "Elements/"
ResConst.BlueElement = ElementDir .. "Blue/BlueElement.prefab"
ResConst.RedElement = ElementDir .. "Red/RedElement.prefab"
ResConst.YellowElement = ElementDir .. "Yellow/YellowElement.prefab"
ResConst.BrownElement = ElementDir .. "Brown/BrownElement.prefab"
ResConst.PurpleElement = ElementDir .. "Purple/PurpleElement.prefab"

ResConst.ElementSprites = {
    [CS.GameCore.ElementType.Blue] = "Assets/Res/Elements/Blue/Blue.png",
    [CS.GameCore.ElementType.Brown] = "Assets/Res/Elements/Brown/Brown.png",
    [CS.GameCore.ElementType.Purple] = "Assets/Res/Elements/Purple/Purple.png",
    [CS.GameCore.ElementType.Red] = "Assets/Res/Elements/Red/Red.png",
    [CS.GameCore.ElementType.Yellow] = "Assets/Res/Elements/Yellow/Yellow.png",
}

ResConst.Container = AssetsDir .. "Container.prefab"

return ResConst
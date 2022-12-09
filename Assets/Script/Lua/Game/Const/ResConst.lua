local ResConst = {}

local AssetsDir = CSE.Application.dataPath + "/Res/"

local ElementDir = AssetsDir + "Element/"
ResConst.BlueElement = ElementDir + "Blue/BlueElement.prefab"
ResConst.RedElement = ElementDir + "Red/RedElement.prefab"
ResConst.YellowElement = ElementDir + "Yellow/YellowElement.prefab"
ResConst.BrownElement = ElementDir + "Brown/BrownElement.prefab"
ResConst.PurpleElement = ElementDir + "Purple/PurpleElement.prefab"

ResConst.Container = AssetsDir + "Container.prefab"

return ResConst
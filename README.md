# TripleTown

### Project Environment

**Unity Version**: 2020.3.33f1

#### 2023.1.6 Update

- Added UI node ResultPanel

- Fixed bug in LuaTimer.lua that funtion still run after disposing

- Added Retry and Quit button and scrips

- Updated LuaBundles and Bundles

---

#### 2023.1.5 Update

- Fixed that regenerated elements did not set Container as parent

- Used `LuaTimer.lua` to replace `Timer.lua`

- Used `LuaTimer:ListenRepeat` to replace Coroutine to control element town animation

- Updated LuaBundle

---

#### 2023.1.4 Update

- Removed unnecessary scripts like XLua examples

- Regenerated XLua Warps

- Rebuild application

- Added Text.RegularExpressions test function in Odin Inspector

---

#### 2023.1.3 Update

- Completed `LuaTimer.lua` with simple test

- Fixed bug of `Scheduler.cs` that lateupdates do not execute

- Removed function `ListenUpdateSeconds` in `Scheduler.cs`

- Deleted some XLua tutorial scripts

- Updated lua byte files

- Updated XLua Wrap

---

#### 2023.1.2 Update

- Used 64bit luac to convert `.lua` to `.lua.bytes`

- Modified `LuaBundleTool.cs`

- Deleted some XLua tutorial scripts

- Added `LuaTimer.lua`

---

#### 2022.12.31 Update(Happy New Year)

- Modified `Container.lua` to enable/disable mouse click on elements when judge start

- Modified `UIBtnItem.lua` to control time interval of using items

---

#### 2022.12.30 Update

- Preparation for lua scripts updating

- Modified `ProjectEnv.cs` to fit XLua loader to load lua script from different path

- Modifed Odin tools of assets and lua scripts exporting to convert `.lua` to `.lua.bytes`

- Deleted `Test.lua` and `Test.cs`

---

#### 2022.12.29 Update

- Added `TweenExtensions` to `LuaCallCSharp`

- Simple calculation for combo and score

---

#### 2022.12.28 Update

- Finished Tools RandomLine and RandomType

- Updated bundles

- Added `BundleList.json` to Test Bundle Hotfix

---

#### 2022.12.27 Update

- Adjust startup sequence of UI node

- Added Start Button to Start up

- Deleted useless script `DataConst.cs`

- Modified `LuaBehaviourInspector.cs` to let child node do not display button Save on inspector

- Added start game flow in `GameManager.lua`

---

#### 2022.12.26 Update

- Tided `Container.lua`

- Added simple base data structure `List.lua` with some methods

- Added repeat function in `Scheduler.cs` to repeat call method with intervals

- Added GameConst.lua to declare some constant variables

- Added a simple Inventory System
  
  - Data: `InventoryManager.lua`, `Inventory.lua`
  
  - UI: `UIBtnItem.lua`, `UIBtnInventory.lua`

- Updated bundles

- Deleted scene `menu.uinty`

---

#### 2022.12.17 Update

- Added documents

---

#### 2022.12.16 Update

- Added lua extension `table.shuffle()`

- Added functions to shuffle elements when game is dead

- Added nodes for Canvas

---

#### 2022.12.15 Update

- Fixed bug when element did not move down after town over

- Fixed bug when new elements for one column did not set to table `Conatainer.lua:elements`

- Modified `Container.lua` to continuously check town, restore elements

- Added function to check wether game is dead in `Container.lua` 

- Set elements' parent to container

- Set `LuaBehaviour.cs` to stop all coroutine when game object destroyed

- Removed `TestClass.lua`

---

#### 2022.12.14 Update

- Modified `Element.lua:_TownCoroutine()` to send elements' game objects to ObjectTool after town animation ending

- Modified `Element.lua:SetPos()` to make row and column settings of elements conform to human habits

- Added `TownCountMinus() ` and Modified `_SwapCoroutine()` to wait town play over in `Container.lua`

- Added function `_ResetColumnLack()` to make a table record count of town in columns

- Updated `Container.lua` to check all elements for town

- Added `Container.lua:_DoTown()` to play town

- Added `Vector2.lua`

---

#### 2022.12.13 Update

- Modified `XLuaRequireType.cs` name to `XLuaLuaCallCSharpType.cs`

- Modified `XLuaCSharpCallLua.cs` name to `XLuaCSharpCallLuaType.cs`

- Modified `AssetsManager.cs` to stop all coroutine when destroy but some assets load coroutine is still running

- Added DOTween APIs to XLua Wrap

- Added Coroutines to swap elements and play town animations in `Container.lua` and `Element.lua`

- Added some comments

- Added some lua table extension in `polyfill.lua`

- Added basic logic of generate elements , swap and town in `Container.lua`

- Put `Container.lua` to use Singleton mode

- Removed `SimpleCoroutineRunner.cs` and its Wrap

---

#### 2022.12.12 Update

- Modified sprites' mode of elements from `Multiple` to `Single`

- Modified `Class.lua` function `NewFromCS` to fit `LuaBehaivour.cs` to inject fields with LuaTable injections

- Modified `AssetsManager.cs` to cancel pack/unpack when call function `LoadAssetBundle`

- Added LuaScript `Coroutine.lua` to create `IEnumerator` in lua

- Added lua generate configuration file `XLuaBlackList.cs` and `XLuaRequireType.cs`

- Updated asset bundles

- Removed XLua Docs

---

#### 2022.12.11 Update

- Modified `AssetsManager.cs` to be fit to call function `LoadAsset` in lua

- Modified `Boot.cs` to wait for `AssetsManager.Instance != null`

- Added Ignored fields and functions into `Generator.cs` field `BlackList`

- Added new function `NewFromCS` for `Class.lua` to use to create LuaClass with `UnityEngine.Object`
  
  ---

#### Before 2022.12.10 Update

- Added base assets, scripts, project settings

- Added asset bundles

#### Implements

[Tencent/xLua](https://github.com/Tencent/xLua)

[XINCGer/LitJson4Unity](https://github.com/XINCGer/LitJson4Unity)

[DOTween](http://dotween.demigiant.com/)

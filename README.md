# TripleTown

#### 2022.12.12 Update

- Modified sprites' mode of elements from `Multiple` to `Single`

- Modified `Class.lua` function `NewFromCS` to fit `LuaBehaivour.cs` to inject fields with LuaTable injections

- Modified `AssetsManager.cs` to cancel pack/unpack when call function `LoadAssetBundle`

- Add LuaScript `Coroutine.lua` to create `IEnumerator` in lua

- Add lua generate configuration file `XLuaBlackList.cs` and `XLuaRequireType.cs`

- Update asset bundles

- Remove Xlua Docs

---

#### 2022.12.11 Update

- Update `AssetsManager.cs` to be fit to call function `LoadAsset` in XLua

- Modified `Boot.cs` for `AssetsManager.Instance != null`

- Add Ignored fields and functions into `Generator.cs` field `BlackList`

- Add new function `NewFromCS` for `Class.lua` to use to create LuaClass with `UnityEngine.Object`

 ---

#### Before 2022.12.10 Update

- Add base assets, scripts, project settings

- Add asset bundles

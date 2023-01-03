#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    
    public class XLuaTestMyEnumWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(XLuaTest.MyEnum), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(XLuaTest.MyEnum), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(XLuaTest.MyEnum), L, null, 3, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "E1", XLuaTest.MyEnum.E1);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "E2", XLuaTest.MyEnum.E2);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(XLuaTest.MyEnum), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushXLuaTestMyEnum(L, (XLuaTest.MyEnum)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "E1"))
                {
                    translator.PushXLuaTestMyEnum(L, XLuaTest.MyEnum.E1);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "E2"))
                {
                    translator.PushXLuaTestMyEnum(L, XLuaTest.MyEnum.E2);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for XLuaTest.MyEnum!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for XLuaTest.MyEnum! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class GameCoreElementTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(GameCore.ElementType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(GameCore.ElementType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(GameCore.ElementType), L, null, 6, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Blue", GameCore.ElementType.Blue);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Brown", GameCore.ElementType.Brown);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Yellow", GameCore.ElementType.Yellow);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Purple", GameCore.ElementType.Purple);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Red", GameCore.ElementType.Red);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(GameCore.ElementType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushGameCoreElementType(L, (GameCore.ElementType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Blue"))
                {
                    translator.PushGameCoreElementType(L, GameCore.ElementType.Blue);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Brown"))
                {
                    translator.PushGameCoreElementType(L, GameCore.ElementType.Brown);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Yellow"))
                {
                    translator.PushGameCoreElementType(L, GameCore.ElementType.Yellow);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Purple"))
                {
                    translator.PushGameCoreElementType(L, GameCore.ElementType.Purple);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Red"))
                {
                    translator.PushGameCoreElementType(L, GameCore.ElementType.Red);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for GameCore.ElementType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for GameCore.ElementType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class GameCoreToolTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(GameCore.ToolType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(GameCore.ToolType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(GameCore.ToolType), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Shuffle", GameCore.ToolType.Shuffle);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "RandomLine", GameCore.ToolType.RandomLine);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "RandomType", GameCore.ToolType.RandomType);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(GameCore.ToolType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushGameCoreToolType(L, (GameCore.ToolType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Shuffle"))
                {
                    translator.PushGameCoreToolType(L, GameCore.ToolType.Shuffle);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "RandomLine"))
                {
                    translator.PushGameCoreToolType(L, GameCore.ToolType.RandomLine);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "RandomType"))
                {
                    translator.PushGameCoreToolType(L, GameCore.ToolType.RandomType);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for GameCore.ToolType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for GameCore.ToolType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
}
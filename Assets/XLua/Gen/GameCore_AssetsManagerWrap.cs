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
    public class GameCoreAssetsManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(GameCore.AssetsManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadGameObject", _m_LoadGameObject);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadAsset", _m_LoadAsset);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadScene", _m_LoadScene);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "FreeObject", _m_FreeObject);
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 1, 0);
			
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "Instance", _g_get_Instance);
            
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new GameCore.AssetsManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to GameCore.AssetsManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadGameObject(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GameCore.AssetsManager gen_to_be_invoked = (GameCore.AssetsManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<UnityEngine.GameObject>>(L, 3)) 
                {
                    string _assetPath = LuaAPI.lua_tostring(L, 2);
                    System.Action<UnityEngine.GameObject> _onLoaded = translator.GetDelegate<System.Action<UnityEngine.GameObject>>(L, 3);
                    
                    gen_to_be_invoked.LoadGameObject( _assetPath, _onLoaded );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _assetPath = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.LoadGameObject( _assetPath );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to GameCore.AssetsManager.LoadGameObject!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadAsset(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GameCore.AssetsManager gen_to_be_invoked = (GameCore.AssetsManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Type>(L, 3)&& translator.Assignable<System.Action<UnityEngine.Object>>(L, 4)) 
                {
                    string _assetPath = LuaAPI.lua_tostring(L, 2);
                    System.Type _type = (System.Type)translator.GetObject(L, 3, typeof(System.Type));
                    System.Action<UnityEngine.Object> _onLoaded = translator.GetDelegate<System.Action<UnityEngine.Object>>(L, 4);
                    
                    gen_to_be_invoked.LoadAsset( _assetPath, _type, _onLoaded );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Type>(L, 3)) 
                {
                    string _assetPath = LuaAPI.lua_tostring(L, 2);
                    System.Type _type = (System.Type)translator.GetObject(L, 3, typeof(System.Type));
                    
                    gen_to_be_invoked.LoadAsset( _assetPath, _type );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to GameCore.AssetsManager.LoadAsset!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadScene(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GameCore.AssetsManager gen_to_be_invoked = (GameCore.AssetsManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _scene = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.LoadScene( _scene );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FreeObject(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GameCore.AssetsManager gen_to_be_invoked = (GameCore.AssetsManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Object>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Object _obj = (UnityEngine.Object)translator.GetObject(L, 2, typeof(UnityEngine.Object));
                    bool _useObjectPool = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.FreeObject( _obj, _useObjectPool );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Object>(L, 2)) 
                {
                    UnityEngine.Object _obj = (UnityEngine.Object)translator.GetObject(L, 2, typeof(UnityEngine.Object));
                    
                    gen_to_be_invoked.FreeObject( _obj );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to GameCore.AssetsManager.FreeObject!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Instance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, GameCore.AssetsManager.Instance);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}

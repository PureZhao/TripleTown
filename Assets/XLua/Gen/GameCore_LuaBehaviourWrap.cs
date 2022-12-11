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
    public class GameCoreLuaBehaviourWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(GameCore.LuaBehaviour);
			Utils.BeginObjectRegister(type, L, translator, 0, 1, 3, 3);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetLuaClass", _m_GetLuaClass);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "requirePath", _g_get_requirePath);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "objects", _g_get_objects);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "values", _g_get_values);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "requirePath", _s_set_requirePath);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "objects", _s_set_objects);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "values", _s_set_values);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 3, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "ValueToJson", _m_ValueToJson_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "JsonToValue", _m_JsonToValue_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new GameCore.LuaBehaviour();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to GameCore.LuaBehaviour constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ValueToJson_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _value = translator.GetObject(L, 1, typeof(object));
                    
                        var gen_ret = GameCore.LuaBehaviour.ValueToJson( _value );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_JsonToValue_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _json = LuaAPI.lua_tostring(L, 1);
                    System.Type _type = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    
                        var gen_ret = GameCore.LuaBehaviour.JsonToValue( _json, _type );
                        translator.PushAny(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLuaClass(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GameCore.LuaBehaviour gen_to_be_invoked = (GameCore.LuaBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetLuaClass(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_requirePath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameCore.LuaBehaviour gen_to_be_invoked = (GameCore.LuaBehaviour)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.requirePath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_objects(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameCore.LuaBehaviour gen_to_be_invoked = (GameCore.LuaBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.objects);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_values(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameCore.LuaBehaviour gen_to_be_invoked = (GameCore.LuaBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.values);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_requirePath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameCore.LuaBehaviour gen_to_be_invoked = (GameCore.LuaBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.requirePath = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_objects(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameCore.LuaBehaviour gen_to_be_invoked = (GameCore.LuaBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.objects = (System.Collections.Generic.List<GameCore.LuaBehaviour.ObjectWrap>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<GameCore.LuaBehaviour.ObjectWrap>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_values(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameCore.LuaBehaviour gen_to_be_invoked = (GameCore.LuaBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.values = (System.Collections.Generic.List<GameCore.LuaBehaviour.ValueWrap>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<GameCore.LuaBehaviour.ValueWrap>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}

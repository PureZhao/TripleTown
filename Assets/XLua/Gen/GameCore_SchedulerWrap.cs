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
    public class GameCoreSchedulerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(GameCore.Scheduler);
			Utils.BeginObjectRegister(type, L, translator, 0, 7, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Delay", _m_Delay);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DelayFrame", _m_DelayFrame);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DelayFrames", _m_DelayFrames);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ListenUpdate", _m_ListenUpdate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ListenLateUpdate", _m_ListenLateUpdate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ListenRepeat", _m_ListenRepeat);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DisposeListener", _m_DisposeListener);
			
			
			
			
			
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
					
					var gen_ret = new GameCore.Scheduler();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to GameCore.Scheduler constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Delay(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GameCore.Scheduler gen_to_be_invoked = (GameCore.Scheduler)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _time = (float)LuaAPI.lua_tonumber(L, 2);
                    System.Action _action = translator.GetDelegate<System.Action>(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.Delay( _time, _action );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DelayFrame(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GameCore.Scheduler gen_to_be_invoked = (GameCore.Scheduler)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action _action = translator.GetDelegate<System.Action>(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.DelayFrame( _action );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DelayFrames(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GameCore.Scheduler gen_to_be_invoked = (GameCore.Scheduler)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _frames = LuaAPI.xlua_tointeger(L, 2);
                    System.Action _action = translator.GetDelegate<System.Action>(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.DelayFrames( _frames, _action );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ListenUpdate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GameCore.Scheduler gen_to_be_invoked = (GameCore.Scheduler)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action _action = translator.GetDelegate<System.Action>(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.ListenUpdate( _action );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ListenLateUpdate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GameCore.Scheduler gen_to_be_invoked = (GameCore.Scheduler)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action _action = translator.GetDelegate<System.Action>(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.ListenLateUpdate( _action );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ListenRepeat(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GameCore.Scheduler gen_to_be_invoked = (GameCore.Scheduler)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _interval = (float)LuaAPI.lua_tonumber(L, 2);
                    int _times = LuaAPI.xlua_tointeger(L, 3);
                    System.Action _action = translator.GetDelegate<System.Action>(L, 4);
                    
                        var gen_ret = gen_to_be_invoked.ListenRepeat( _interval, _times, _action );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DisposeListener(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GameCore.Scheduler gen_to_be_invoked = (GameCore.Scheduler)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    GameCore.ListenerIndentity _handler = (GameCore.ListenerIndentity)translator.GetObject(L, 2, typeof(GameCore.ListenerIndentity));
                    
                    gen_to_be_invoked.DisposeListener( _handler );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Instance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, GameCore.Scheduler.Instance);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}

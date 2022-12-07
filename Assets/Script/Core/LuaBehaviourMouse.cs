using System;
using UnityEngine;
using XLua;

namespace GameCore
{
    public class LuaBehaviourMouse : MonoBehaviour
    {
        private LuaTable luaClass;
        private LuaFunction onMouseDown;
        private LuaFunction onMouseUp;
        private LuaFunction onMouseEnter;
        private LuaFunction onMouseExit;
        private LuaFunction onMouseDrag;
        private LuaFunction onMouseOver;
        public void BindEvent(LuaTable table)
        {
            luaClass = table;
            onMouseDown = table.Get<LuaFunction>("OnMouseDown");
            onMouseUp = table.Get<LuaFunction>("OnMouseUp");
            onMouseEnter = table.Get<LuaFunction>("OnMouseEnter");
            onMouseExit = table.Get<LuaFunction>("OnMouseExit");
            onMouseDrag = table.Get<LuaFunction>("OnMouseDrag");
            onMouseOver = table.Get<LuaFunction>("OnMouseOver");
        }
        private void OnMouseDown()
        {
            onMouseDown?.Call(luaClass);
        }
        private void OnMouseUp()
        {
            onMouseUp?.Call(luaClass);
        }

        private void OnMouseDrag()
        {
            onMouseDrag?.Call(luaClass);
        }

        private void OnMouseEnter()
        {
            onMouseEnter?.Call(luaClass);
        }
        private void OnMouseExit()
        {
            onMouseExit?.Call(luaClass);
        }
        private void OnMouseOver()
        {
            onMouseOver?.Call(luaClass);
        }

        private void OnDestroy()
        {
            onMouseDown = null;
            onMouseUp = null;
            onMouseEnter = null;
            onMouseExit = null;
            onMouseDrag = null;
            onMouseOver = null;
            luaClass = null;
        }
    }
}
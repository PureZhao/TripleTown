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
        private bool isEventListen = false;

        public void ActivateEvent(bool state)
        {
            isEventListen = state;
        }
        public void BindEvent(LuaTable table)
        {
            luaClass = table;
            onMouseDown = table.Get<LuaFunction>("OnMouseDown");
            onMouseUp = table.Get<LuaFunction>("OnMouseUp");
            onMouseEnter = table.Get<LuaFunction>("OnMouseEnter");
            onMouseExit = table.Get<LuaFunction>("OnMouseExit");
            onMouseDrag = table.Get<LuaFunction>("OnMouseDrag");
            onMouseOver = table.Get<LuaFunction>("OnMouseOver");
            isEventListen = true;
        }
        private void OnMouseDown()
        {
            if (!isEventListen) return;
            onMouseDown?.Call(luaClass);
        }
        private void OnMouseUp()
        {
            if (!isEventListen) return;
            onMouseUp?.Call(luaClass);
        }

        private void OnMouseDrag()
        {
            if (!isEventListen) return;
            onMouseDrag?.Call(luaClass);
        }

        private void OnMouseEnter()
        {
            if (!isEventListen) return;
            onMouseEnter?.Call(luaClass);
        }
        private void OnMouseExit()
        {
            if (!isEventListen) return;
            onMouseExit?.Call(luaClass);
        }
        private void OnMouseOver()
        {
            if (!isEventListen) return;
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
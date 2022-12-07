using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace GameCore
{
    public class EventTarget
    {
        public enum InvokeType
        {
            Normal,
            Once,
        }
        public class HandlerInfo
        {
            public string type;
            public int hash;
        }
        class Handler
        {
            private event Action action;
            private bool isDelete;
            private InvokeType type;
            public void Set(Action a, InvokeType it)
            {
                action = a;
                type = it;
                isDelete = false;
            }
            public void Execute()
            {
                if (isDelete) return;
                isDelete = type == InvokeType.Once;
                action?.Invoke();
            }

            public int GetHash()
            {
                return action.GetHashCode();
            }

            public void Dispose()
            {
                isDelete = true;
            }
        }

        Dictionary<string, List<Handler>> handlers;
        public EventTarget()
        {
            handlers = new Dictionary<string, List<Handler>>();
        }

        ~EventTarget()
        {
            foreach (List<Handler> handlerList in handlers.Values)
            {
                handlerList.Clear();
            }
            handlers.Clear();
        }

        private List<Handler> CheckType(string type)
        {
            if (!handlers.ContainsKey(type))
            {
                handlers.Add(type, new List<Handler>());
            }
            return handlers[type];
        }

        private HandlerInfo Add(string type, Action action, InvokeType invokeType)
        {
            Handler handler = new Handler();
            handler.Set(action, invokeType);
            CheckType(type).Add(handler);
            HandlerInfo info = new HandlerInfo()
            {
                type = type,
                hash = action.GetHashCode(),
            };
            return info;
        }

        public HandlerInfo Listen(string type, Action action)
        {
            return Add(type, action, InvokeType.Normal);
        }

        public HandlerInfo ListenOnce(string type, Action action)
        {
            return Add(type, action, InvokeType.Once);
        }

        public void Emit(string type)
        {
            if (!handlers.ContainsKey(type))
            {
                Debug.LogError("No Type Of " + type);
                return;
            }
            List<Handler> list = handlers[type];
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Execute();
            }
        }

        public void DisposeHandler(HandlerInfo info)
        {
            try
            {
                List<Handler> events = handlers[info.type];
                for(int i = 0; i < events.Count; i++)
                {
                    if (events[i].GetHash() == info.hash)
                    {
                        events[i].Dispose();
                        break;
                    }
                }
            }
            catch(Exception e)
            {
                Debug.LogError(e);
            }
        }

        public void RemoveHandler(HandlerInfo info)
        {
            try
            {
                List<Handler> events = handlers[info.type];
                int index = -1;
                for (int i = 0; i < events.Count; i++)
                {
                    if (events[i].GetHash() == info.hash)
                    {
                        index = i;
                        break;
                    }
                }
                if (index != -1)
                {
                    events.RemoveAt(index);
                }
                Debug.Log(events.Count);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public void RemoveByType(string type)
        {
            try
            {
                List<Handler> events = handlers[type];
                events.Clear();
            }
            catch(Exception e)
            {
                Debug.LogError(e);
            }
        }

        public void RemoveAll()
        {
            foreach (List<Handler> handlerList in handlers.Values)
            {
                handlerList.Clear();
            }
            handlers.Clear();
        }

    }
}

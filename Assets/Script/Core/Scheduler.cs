using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XLua;

namespace GameCore
{
    public enum ListenerType
    {
        Update,
        UpdateSecond,
        LateUpdate,
        Delay,
        DelayFrame,
        Repeat,
    }
    public class ListenerIndentity
    {
        public ListenerType type;
        public Listener listener;
    }
    [LuaCallCSharp]
    public class Scheduler : MonoBehaviour
    {
        private static Scheduler instance;

        public static Scheduler Instance { get => instance; }

        List<Listener> updates = new List<Listener>();
        List<Listener> updateSeconds = new List<Listener>();
        List<Listener> lateUpdates = new List<Listener>();
        List<Listener> curFrame = new List<Listener>();
        List<Listener> lastFrame = new List<Listener>();
        List<Listener> delaies = new List<Listener>();
        List<Listener> repeats = new List<Listener>();

        List<Listener> executing = new List<Listener>();

        void Awake()
        {
            
            if(instance != null)
            {
                Destroy(this);
            }
            else
            {
                instance = this;
            }
            DontDestroyOnLoad(this);
        }

        private void ClearNull(ref List<Listener> col)
        {
            
            List<Listener> t = new List<Listener>();
            for(int i = 0; i < col.Count; i++)
            {
                if (col[i] != null)
                {
                    t.Add(col[i]);
                }
            }
            col.Clear();
            col = t;
        }
        public ListenerIndentity Delay(float time, Action action)
        {
            Listener delay = new Listener()
            {
                callTime = Time.realtimeSinceStartup + time,
                action = action,
            };
            delaies.Add(delay);
            ListenerIndentity indentity = new ListenerIndentity()
            {
                listener = delay,
                type = ListenerType.Delay,
            };
            return indentity;
        }

        public ListenerIndentity DelayFrame(Action action)
        {
            Listener delayFrame = new Listener()
            {
                leftFrames = 1,
                action = action,
            };
            curFrame.Add(delayFrame);
            ListenerIndentity indentity = new ListenerIndentity()
            {
                listener = delayFrame,
                type = ListenerType.DelayFrame,
            };
            return indentity;
        }

        public ListenerIndentity DelayFrames(int frames, Action action)
        {
            if(frames < 1)
            {
                frames = 1;
            }
            Listener delayFrames = new Listener()
            {
                leftFrames = frames,
                action = action,
            };
            curFrame.Add(delayFrames);
            ListenerIndentity indentity = new ListenerIndentity()
            {
                listener = delayFrames,
                type = ListenerType.DelayFrame,
            };
            return indentity;
        }

        public ListenerIndentity ListenUpdate(Action action)
        {
            Listener update = new Listener()
            {
                action = action,
            };
            updates.Add(update);
            ListenerIndentity indentity = new ListenerIndentity()
            {
                listener = update,
                type = ListenerType.Update,
            };
            return indentity;
        }

        public ListenerIndentity ListenLateUpdate(Action action)
        {
            Listener lateUpdate = new Listener()
            {
                action = action,
            };
            lateUpdates.Add(lateUpdate);
            ListenerIndentity indentity = new ListenerIndentity()
            {
                listener = lateUpdate,
                type = ListenerType.LateUpdate,
            };
            return indentity;
        }

        public ListenerIndentity ListenUpdateSeconds(float time, Action action)
        {
            Listener update = new Listener()
            {
                callTime = time + Time.realtimeSinceStartup,
                action = action,
            };
            updateSeconds.Add(update);
            ListenerIndentity indentity = new ListenerIndentity()
            {
                listener = update,
                type = ListenerType.UpdateSecond,
            };
            return indentity;
        }

        public ListenerIndentity ListenRepeat(float interval, int times, Action action)
        {
            Listener listener = new Listener()
            {
                interval = interval,
                callTime = interval + Time.realtimeSinceStartup,
                times = times,
                action = action,
            };
            repeats.Add(listener);
            return new ListenerIndentity()
            {
                listener = listener,
                type = ListenerType.Repeat
            };
        }
        public void DisposeListener(ListenerIndentity handler)
        {
            List<Listener> collect = null;
            switch (handler.type)
            {
                case ListenerType.Delay: collect = delaies; break;
                case ListenerType.DelayFrame: collect = curFrame; break;
                case ListenerType.LateUpdate: collect = lateUpdates; break;
                case ListenerType.Update: collect = updates; break;
                case ListenerType.UpdateSecond: collect = updateSeconds; break;
                case ListenerType.Repeat: collect = repeats; break;
            }
            if(collect != null)
            {
                for(int i = 0; i < collect.Count; i++)
                {
                    if (collect[i] == handler.listener)
                    {
                        collect[i].action = null;
                        collect[i] = null;
                    }
                }
            }
            ClearNull(ref collect);
        }

        void Update()
        {
            executing.Clear();
            // delay
            float now = Time.realtimeSinceStartup;
            for(int i = 0; i < delaies.Count; i++)
            {
                if(now >= delaies[i].callTime)
                {
                    executing.Add(delaies[i]);
                    delaies[i] = null;
                }
            }
            ClearNull(ref delaies);
            // DelayFrames
            executing.AddRange(lastFrame);
            lastFrame.Clear();

            // update
            executing.AddRange(updates);
            // updateSeconds
            for(int i = 0; i < updateSeconds.Count; i++)
            {
                if(now < updateSeconds[i].callTime)
                {
                    executing.Add(updateSeconds[i]);
                }
                else
                {
                    updateSeconds[i] = null;
                }
            }
            ClearNull(ref updateSeconds);
            // repeat
            for(int i = 0;i < repeats.Count; i++)
            {
                if(now >= repeats[i].callTime && repeats[i].times != 0)
                {
                    executing.Add(repeats[i]);
                    if (repeats[i].times > 0)
                    {
                        repeats[i].times--;
                        repeats[i].callTime += repeats[i].interval;
                    }
                }
                else
                {
                    if (repeats[i].times == 0)
                    {
                        repeats[i] = null;
                    }
                    
                }
            }
            ClearNull(ref repeats);

            for(int i = 0; i < executing.Count; i++)
            {
                executing[i]?.Invoke();
            }
            
        }

        void LateUpdate()
        {
            executing.Clear();
            executing.AddRange(lateUpdates);

            lastFrame.Clear();
            for(int i = 0; i < curFrame.Count; i++)
            {
                curFrame[i].leftFrames--;
                if (curFrame[i].leftFrames <= 0)
                {
                    lastFrame.Add(curFrame[i]);
                    curFrame[i] = null;
                }
            }
            ClearNull(ref curFrame);
        }

        private void OnDisable()
        {
            updates.Clear();
            updateSeconds.Clear();
            lateUpdates.Clear();
            curFrame.Clear();
            lastFrame.Clear();
            delaies.Clear();
        }

        private void OnDestroy()
        {
            instance = null;
        }

    }
}
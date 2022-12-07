using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace GameCore
{
    public class Listener
    {
        public Action action;
        public bool isDelete;
        public int leftFrames;
        public float callTime;
        public void Invoke()
        {
            if (!isDelete)
            {
                action?.Invoke();
            }
        }

    }
}
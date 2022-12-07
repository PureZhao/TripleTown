using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GameCore
{
    public class Boot : MonoBehaviour
    {
        private void Start()
        {
            string cmd = string.Format("local t = require('{0}'); return t;", "Boot");
            ProjectLuaEnv.Instance.DoString(cmd);
        }


        private void OnDestroy()
        {
            ProjectLuaEnv.Instance.Dispose();
        }
    }
}
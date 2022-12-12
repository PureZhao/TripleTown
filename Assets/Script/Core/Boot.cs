using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class Boot : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return new WaitUntil(() =>
            {
                return AssetsManager.Instance != null 
                && GameObjectPool.Instane != null
                && Scheduler.Instance != null;
            });
            string cmd = string.Format("local t = require('{0}'); return t;", "Boot");
            ProjectLuaEnv.Instance.DoString(cmd);
        }


        private void OnDestroy()
        {
            ProjectLuaEnv.Instance.Dispose();
        }
    }
}
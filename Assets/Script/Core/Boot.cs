using DG.Tweening;
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
            string cmd = "require('Core.Global'); require('Boot');";
            ProjectLuaEnv.Instance.DoString(cmd);

        }


        private void OnDestroy()
        {
            ProjectLuaEnv.Instance.Dispose();
        }
    }
}
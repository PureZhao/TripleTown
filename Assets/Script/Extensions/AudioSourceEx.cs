using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public static class AudioSourceEx
{
    public static void PlayOnce(this AudioSource source, Action onCompleted = null)
    {
        if (source.clip == null) return;
        bool loopState = source.loop;
        source.loop = false;
        source.Play();
        if(onCompleted != null)
        {
            int clipLength = Mathf.CeilToInt(source.clip.length * 1000f);
            Task.Run(async delegate
            {
                await Task.Delay(clipLength);
                onCompleted?.Invoke();
                source.loop = loopState;
            });
        }
    }
}

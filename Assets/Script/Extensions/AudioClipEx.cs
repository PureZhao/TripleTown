using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Timers;

public static class AudioClipEx
{
    public static void PlayAtPoint(this AudioClip clip, Vector3 position, Action onCompleted = null)
    {
        if (clip == null) return;
        AudioSource.PlayClipAtPoint(clip, position);
        if(onCompleted != null)
        {
            int time = Mathf.CeilToInt(clip.length * 1000f);
            Task.Run(async delegate
            {
                await Task.Delay(time);
                onCompleted?.Invoke();
            });
        }
    }
}

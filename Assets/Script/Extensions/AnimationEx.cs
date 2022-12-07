using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public static class AnimationEx
{
    public static AnimationState GetAnimationState(this Animation animation, string clipName)
    {
        return animation[clipName];
    }
    public static List<AnimationClip> GetAllClips(this Animation animation)
    {
        List<AnimationClip> clips = new List<AnimationClip>(); 
        foreach(AnimationState state in animation)
        {
            clips.Add(state.clip);
        }
        return clips;
    }

    public static void PlayOnce(this Animation animation, Action onCompleted = null)
    {
        animation.Play();
        if (onCompleted != null)
        {
            AnimationClip clip = animation.clip;
            int clipLength = Mathf.CeilToInt(clip.length * 1000f);
            Task.Run(async delegate
            {
                await Task.Delay(clipLength);
                onCompleted?.Invoke();
            });
        }
    }
}

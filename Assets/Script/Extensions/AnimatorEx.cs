using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif
using UnityEngine;

public static class AnimatorEx
{
#if UNITY_EDITOR
    public static List<AnimatorState> GetAllStates(this Animator animator)
    {

        List<AnimatorState> states = new List<AnimatorState>();
        AnimatorController controller = animator.runtimeAnimatorController as AnimatorController;
        AnimatorControllerLayer[] layers = controller.layers;
        foreach(var layer in layers)
        {
            AnimatorStateMachine stateMachine = layer.stateMachine;
            foreach(var state in stateMachine.states)
            {
                states.Add(state.state);
            }
        }
        return states;
    }

    public static List<AnimationClip> GetAllAnimationClips(this Animator animator)
    {
        List<AnimationClip> clips = new List<AnimationClip>();
        AnimatorController controller = animator.runtimeAnimatorController as AnimatorController;
        clips.AddRange(controller.animationClips);
        return clips;
    }


#endif

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System;
using DG.Tweening;
public static class XLuaLuaCallCSharpType
{
    [LuaCallCSharp]
    public static List<Type> LuaCallCharp = new List<Type>()
    {
        typeof(System.Object),
        typeof(UnityEngine.Object),
        typeof(Vector2),
        typeof(Vector3),
        typeof(Vector4),
        typeof(Quaternion),
        typeof(Color),
        typeof(Ray),
        typeof(Bounds),
        typeof(Ray2D),
        typeof(Time),
        typeof(GameObject),
        typeof(Component),
        typeof(Behaviour),
        typeof(Transform),
        typeof(Resources),
        typeof(TextAsset),
        typeof(Keyframe),
        typeof(AnimationCurve),
        typeof(AnimationClip),
        typeof(MonoBehaviour),
        typeof(ParticleSystem),
        typeof(SkinnedMeshRenderer),
        typeof(Renderer),
        typeof(WWW),
        typeof(Light),
        typeof(Mathf),
        typeof(System.Collections.Generic.List<int>),
        typeof(Action<string>),
        typeof(UnityEngine.Debug),
        typeof(WaitForSeconds),
        typeof(WaitUntil),
        typeof(Rect),
        typeof(Sprite),
        typeof(ShortcutExtensions),
        typeof(TweenSettingsExtensions),
        typeof(DOTweenModuleUI),
        typeof(TweenExtensions),
    };
}

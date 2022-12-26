using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Sprite sprite;
    private IEnumerator Start()
    {
        yield return new WaitUntil(() =>
        {
            return AssetsManager.Instance != null;
        });
    }
}

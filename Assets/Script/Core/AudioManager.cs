using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;

public class AudioManager
{
    private static AudioManager instance;
    private static AudioManager Instance
    {
        get
        {
            instance ??= new AudioManager();
            return instance;
        }
    }
    private GameObject sourceTemplate;
    private AudioManager()
    {
        sourceTemplate = new GameObject("AudioSource Template");
        sourceTemplate.GetOrAddComponent<AudioSource>((comp) =>
        {
            comp.playOnAwake = false;
        });
        GameObject.DontDestroyOnLoad(sourceTemplate);
    }

    ~AudioManager()
    {
        GameObject.Destroy(sourceTemplate);
    }
    public static void Play(string clipPath, AudioSource source = null)
    {
        AssetsManager.Instance.LoadAsset<AudioClip>(clipPath, (clip) =>
        {
            
            if (source == null)
            {
                GameObject g = GameObject.Instantiate(Instance.sourceTemplate, Vector3.zero, Quaternion.identity);
                source = g.GetOrAddComponent<AudioSource>();
                source.clip = clip;
                source.Play();
                GameObject.Destroy(g, clip.length + 0.5f);
            }
            else
            {
                source.clip = clip;
                source.Play();
            }
            
        });
    }
}

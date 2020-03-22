using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SOUNDS {  };

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{


    public GameAudio[] audioClipsArr;

    private Dictionary<SOUNDS, AudioClip> audioClipsDic;

    private AudioSource aS;

    private static SoundManager instance;


    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("Audiomanager instance was null!");
            }
            return instance;
        }
    }



    private void Awake()
    {
        instance = this;

        audioClipsDic = new Dictionary<SOUNDS, AudioClip>();

        foreach (var item in audioClipsArr)
        {
            audioClipsDic.Add(item.name, item.audio);
        }

    }
    private void Start()
    {
        aS = GetComponent<AudioSource>();

        DontDestroyOnLoad(this.gameObject);
     
    }

    private void _PlayAudio(SOUNDS sound, float volume)
    {
        aS.PlayOneShot(audioClipsDic[sound], volume);
    }

    public static void PlayAudio(SOUNDS sound, float volume = 1f)
    {
        if (instance == null)
            return;

        instance._PlayAudio(sound, volume);
    }

    public static void PlayAudio(AudioClip sound, float volume = 1f)
    {
        if (instance == null)
            return;

        instance.aS.PlayOneShot(sound, volume);
    }

    private void OnDestroy()
    {

    }

}

[Serializable]
public struct GameAudio
{
    public SOUNDS name;
    public AudioClip audio;
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SOUNDS { EAT_SOUND, INVALID_INPUT };

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

     
    }

    private void _PlayAudio(SOUNDS sound, float volume = 1)
    {
        aS.pitch += UnityEngine.Random.Range(-0.2f, 0.2f);
        aS.PlayOneShot(audioClipsDic[sound], volume * SaveManager.Settings.soundEffectVolume);
        aS.pitch = 1;

    }
    private void _PlayAudio(AudioClip sound, float volume = 1)
    {
        aS.pitch = 1;
        aS.pitch += UnityEngine.Random.Range(-0.2f, 0.2f);
        aS.PlayOneShot(sound, volume * SaveManager.Settings.soundEffectVolume);

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

        instance._PlayAudio(sound, volume);
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

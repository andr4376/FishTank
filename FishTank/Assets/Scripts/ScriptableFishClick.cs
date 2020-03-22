using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="FishClick", menuName ="ScriptableObjects/FishClick")]
public class ScriptableFishClick : ScriptableObject
{
    public int reward;

    public AudioClip sounds;
    public float soundVolume;

    public float coolDown;
}

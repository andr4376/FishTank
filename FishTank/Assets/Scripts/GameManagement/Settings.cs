using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A serializable class that contains info about the 
/// audio, graphical and gameplay settings of the game
/// </summary>
[System.Serializable]
public class Settings 
{
    public float musicVolume = 0.5f;
    public float soundEffectVolume = 2.5f;

    public bool volumetricLighting = true;

    public bool peaceMode = false;

    public float fogDistance = 1;
}

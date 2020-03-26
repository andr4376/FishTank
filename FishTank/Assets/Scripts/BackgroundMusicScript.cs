using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusicScript : MonoBehaviour
{

    private AudioSource aS;


    // Start is called before the first frame update
    void Start()
    {
        aS = GetComponent<AudioSource>();

                  
           aS.volume = SaveManager.Settings.musicVolume;

        SettingsUpdateScript.onSettingsChanged += delegate ()
        {
            UpdateVolume();
        };

    }

    private void UpdateVolume()
    {
        aS.volume = 1 * SaveManager.Settings.musicVolume;

    }



}

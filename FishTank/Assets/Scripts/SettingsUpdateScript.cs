using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUpdateScript : MonoBehaviour
{
    public delegate void OnSettingsChanged();
    public static OnSettingsChanged onSettingsChanged;




    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundSlider;
    [SerializeField] Slider fogslider;
    [SerializeField] Toggle volumetricToggle;
    [SerializeField] Toggle peaceToggle;

    private bool hasLoaded = false;

    private VolumetricLightScatteringEffect volumetricLight;

    private void Awake()
    {
        Load();
    }

    private void Load()
    {
       
            musicSlider.value = SaveManager.Settings.musicVolume;
            soundSlider.value = SaveManager.Settings.soundEffectVolume;
            fogslider.value = SaveManager.Settings.fogDistance;

            volumetricToggle.isOn = SaveManager.Settings.volumetricLighting;
            peaceToggle.isOn = SaveManager.Settings.peaceMode;


        
            hasLoaded = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        volumetricLight = Camera.main.GetComponent<VolumetricLightScatteringEffect>();

        onSettingsChanged += delegate ()
         {
             UpdateSettings();
         };

        Load();

    }

    private void UpdateSettings()
    {
        volumetricLight.enabled = SaveManager.Settings.volumetricLighting;


    }



    public void SettingsHasChanged()
    {
        if (hasLoaded)
        {
            SaveManager.Settings.fogDistance = fogslider.value;
            SaveManager.Settings.musicVolume = musicSlider.value;
            SaveManager.Settings.soundEffectVolume = soundSlider.value;
            SaveManager.Settings.volumetricLighting = volumetricToggle.isOn;
            SaveManager.Settings.peaceMode = peaceToggle.isOn;
            onSettingsChanged?.Invoke();
        }
    }
}

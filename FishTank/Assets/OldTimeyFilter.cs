using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldTimeyFilter : MonoBehaviour
{
    public FilterSettings filterSettings;

    private FilterSettings initialSettings;


    private VignetteEffect vignette;
    private ColorAdjustmentEffect colorAdjuster;
    private StaticEffect staticEffect;

    private bool isOn = false;

    // Start is called before the first frame update
    void Start()
    {
        Camera cam = Camera.main;

        vignette = cam.GetComponent<VignetteEffect>();
        colorAdjuster = cam.GetComponent<ColorAdjustmentEffect>();
        staticEffect = cam.GetComponent<StaticEffect>();

        initialSettings = new FilterSettings(vignette._strength, vignette._size, colorAdjuster._saturation, colorAdjuster._contrast, colorAdjuster._brightness);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        isOn = !isOn;

        ApplyFilter(
            isOn ? filterSettings : initialSettings);
    }

    private void ApplyFilter(FilterSettings filter)
    {
        vignette._strength = filter.vignetteStrenght;
        vignette._size = filter.vignetteSize;

        colorAdjuster._contrast = filter.contrast;
        colorAdjuster._brightness = filter.brightness;
        colorAdjuster._saturation = filter.saturation;

        staticEffect.enabled = isOn;
    } 
}

[System.Serializable]
public struct FilterSettings
{
    [Range(0, 1)]
    public float vignetteStrenght, vignetteSize;

    [Range(0, 2)]
    public float saturation, contrast;

    [Range(-1,1)] public float brightness;

    public FilterSettings(float vignetteStrenght, float vignetteSize, float saturation, float contrast, float brightness)
    {
        this.vignetteStrenght = vignetteStrenght;
        this.vignetteSize = vignetteSize;
        this.saturation = saturation;
        this.contrast = contrast;
        this.brightness = brightness;
    }
}
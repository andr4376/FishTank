﻿
using System;
using UnityEngine;

[ExecuteAlways]
public class FogController : MonoBehaviour
{
    private const string FogColorKeyword = "_FogColor";
    private const string FogStartKeyword = "_FogStart";
    private const string FogEndKeyword = "_FogEnd";
    private const string HeightFogStartKeyword = "_HeightFogStart";
    private const string HeightFogEndKeyword = "_HeightFogEnd";
    private const string _FogIntensityKW = "_FogIntensity";
    private const string _HeightFogIntensityKW = "_HeaightFogStrenght";


    [Header("FOG SETTINGS")]
    [SerializeField] private Color _fogColor;
    [SerializeField] private bool useCamColor;
    [SerializeField] private float _fogStart = 0;
    public float _fogEnd = 150;
    [SerializeField] private float _heightFogStart = 0;
    [SerializeField] private float _heightFogEnd = 10;
    [Range(0, 1)]
    [SerializeField] private float _FogIntensity = 1.2f;

    [Range(0, 2)]
    [SerializeField] private float _HeightFogIntensity = 0.7f;

    private float FogDistance
    {
        get
        {
            float fog = _fogEnd;
            
                fog *= SaveManager.Settings.fogDistance;

                if (fog <= _fogStart)
                    fog = _fogStart;

            return fog;
        }
    }

    private Camera cam;

    public Camera Cam
    {
        get
        {
            if (cam == null)
                cam = Camera.main;

            return cam;
        }
    }
    private void Update()
    {
        UpdateShader();
    }

    private void Start()
    {

        UpdateShader();

        //for prototype fps scene
        HeadWaterSurfaceScript.OnAboveWater e = delegate ()
        {
            ToggleFog();
        };

        HeadWaterSurfaceScript.AddOnAboveWaterEvent(e);
    }

    private void ToggleFog()
    {
        _FogIntensity = _FogIntensity > 0 ? 0 : 1;
    }

    private void UpdateShader()
    {
        _fogColor = useCamColor? 
            Cam.backgroundColor : _fogColor;

        Shader.SetGlobalColor(FogColorKeyword, _fogColor);
        Shader.SetGlobalFloat(FogStartKeyword, _fogStart);



        Shader.SetGlobalFloat(FogEndKeyword, FogDistance);
        Shader.SetGlobalFloat(HeightFogStartKeyword, _heightFogStart);
        Shader.SetGlobalFloat(HeightFogEndKeyword, _heightFogEnd);
        Shader.SetGlobalFloat(_HeightFogIntensityKW, _HeightFogIntensity);
        Shader.SetGlobalFloat(_FogIntensityKW, _FogIntensity);
    }
}

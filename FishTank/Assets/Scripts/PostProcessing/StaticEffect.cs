

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEffect : MonoBehaviour
{
    [SerializeField] private Material _staticEffectMat = default;


    private const string ScrollSpeedKW = "_ScrollSpeed";
    private const string VisibilityKW = "_Visibility";

    [SerializeField, Range(0, 1)] private float visibility = 1;
    [SerializeField] private Vector2 scrollSpeed = Vector2.zero;

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (_staticEffectMat == null)
        {
            return;
        }

        #if UNITY_EDITOR

        _staticEffectMat.SetVector(ScrollSpeedKW, scrollSpeed);
        _staticEffectMat.SetFloat(VisibilityKW, visibility);
        #endif
        Graphics.Blit(src, dst, _staticEffectMat);
    }

    private void Start()
    {
        _staticEffectMat.SetVector(ScrollSpeedKW, scrollSpeed);
        _staticEffectMat.SetFloat(VisibilityKW, visibility);
    }
}

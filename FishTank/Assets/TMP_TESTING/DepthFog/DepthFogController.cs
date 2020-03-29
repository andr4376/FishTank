using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthFogController : MonoBehaviour
{

    public Material material;

    private Camera cam;

    public float fogStart = 10;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        cam.depthTextureMode = DepthTextureMode.Depth;
        
    }

    private void Update()
    {
        material.SetColor("_FogColor", cam.backgroundColor);
        material.SetFloat("_FarClipPlane", cam.farClipPlane);
        material.SetFloat("_FogStart", fogStart);

        
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }
}

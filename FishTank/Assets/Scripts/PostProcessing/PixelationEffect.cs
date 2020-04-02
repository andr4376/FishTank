using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelationEffect : MonoBehaviour
{

    public Material pixelationMaterial;

    private const string resolutionKW = "_Resolution";

    [Range(0,1)]
    public float resolutionPercentage;

    private float x, y;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        if (pixelationMaterial == null)
        {
        pixelationMaterial = new Material(Shader.Find("FishTank/PixelationShader"));

        }

        x = Screen.width * resolutionPercentage;
        y = Screen.height * resolutionPercentage;

        pixelationMaterial.SetVector(resolutionKW, new Vector2(
           x,
           y
           ));
    }

    /*
    void Update()
    {
        x = Screen.width * resolutionPercentage;
        y = Screen.height * resolutionPercentage;

        pixelationMaterial.SetVector(resolutionKW, new Vector2(
           x,
           y
           ));
    }*/

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (pixelationMaterial == null)
        {
            return;
        }

        
        Graphics.Blit(source, destination, pixelationMaterial);

    }
}

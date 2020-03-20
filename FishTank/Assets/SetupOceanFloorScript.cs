using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupOceanFloorScript : MonoBehaviour
{
    [SerializeField]
    Texture2D oceanFloorTexture;

    [SerializeField] Vector3 scroll = new Vector3(-0.5f, -0.5f,0);
    [SerializeField] float threshold = 0.9f;

    [SerializeField][Range(0.1f,0.02f)] float size = 0.1f;
    [SerializeField][Range(0,1)] float transparency = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
        Shader.SetGlobalTexture("_OceanFloorTexture", oceanFloorTexture);
        Shader.SetGlobalFloat("_Threshold", threshold);
        Shader.SetGlobalVector("_TextureScroll", scroll);
        
        Shader.SetGlobalFloat("_TexureTransparency", transparency);

    }

    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalFloat("_Threshold", threshold);
        Shader.SetGlobalVector("_TextureScroll", scroll);
        Shader.SetGlobalFloat("_Size", size);
        Shader.SetGlobalFloat("_TexureTransparency", transparency);

    }
}

//#define DEBUGGING
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if DEBUGGING
[ExecuteAlways]
#endif
public class SetupOceanFloorScript : MonoBehaviour
{
    [SerializeField]
    Texture2D oceanFloorTexture;

    [SerializeField] Vector3 scroll = new Vector3(-0.5f, -0.5f,0);

    [SerializeField] Color lightColor = new Color(1,1,1,1);


    // Start is called before the first frame update
    void Start()
    {
        Shader.SetGlobalTexture("_OceanFloorTexture", oceanFloorTexture);
        Shader.SetGlobalVector("_TextureScroll", scroll);
        
        Shader.SetGlobalColor("_LightColor", lightColor);
    }

#if DEBUGGING
    // Update is called once per frame
    void Update()
    {

        Shader.SetGlobalVector("_TextureScroll", scroll);
        Shader.SetGlobalColor("_LightColor", lightColor);

    }
#endif
}

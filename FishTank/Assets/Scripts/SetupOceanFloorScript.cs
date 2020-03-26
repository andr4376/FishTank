using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SetupOceanFloorScript : MonoBehaviour
{
    [SerializeField]
    Texture2D oceanFloorTexture;

    [SerializeField]
    Texture2D noiseTexture;

    [SerializeField] Vector3 scroll = new Vector3(-0.5f, -0.5f,0);
    [SerializeField] Vector3 noiseScroll = new Vector3(-0.5f, -0.5f,0);

    [SerializeField] [Range(1, 20)] float _Distortion = 1.3f;

    public Color lightColor = new Color(1,1,1,1);




    // Start is called before the first frame update
    void Start()
    {
        Shader.SetGlobalTexture("_OceanFloorTexture", oceanFloorTexture);
        Shader.SetGlobalTexture("_OceanFloorNoiseTexture", noiseTexture);

        Shader.SetGlobalVector("_TextureScroll", scroll);
        Shader.SetGlobalVector("_OceanFloorNoiseScroll", noiseScroll);
        Shader.SetGlobalFloat("_OceanFloorDistortion", _Distortion);

        Shader.SetGlobalColor("_LightColor", lightColor);

    }

    // Update is called once per frame
    void Update()
    {

      //  Shader.SetGlobalVector("_TextureScroll", scroll);
        Shader.SetGlobalColor("_LightColor", lightColor);


        Shader.SetGlobalVector("_OceanFloorNoiseScroll", noiseScroll);
        Shader.SetGlobalFloat("_OceanFloorDistortion", _Distortion);
    }
}

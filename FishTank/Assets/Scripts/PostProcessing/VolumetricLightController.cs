
using UnityEngine;

[ExecuteAlways]
public class VolumetricLightController : MonoBehaviour
{
    private const string VL_RadiusKeyword  = "_Radius";
    private const string VL_DecayKeyword  = "_Decay";
    private const string VL_DensityKeyword   = "_Density";
    private const string VL_WeightKeyword  = "_Weight";
    private const string VL_ExposureKeyword  = "_Exposure";


    [SerializeField] private Material VLMaterial;

    [Header("Volumetric Light Settings")]
    [Range(0,5)]
    [SerializeField] private float _Radius = 2.3f;

    [Range(0, 1.5f)]
    [SerializeField] private float _Decay = 0.971f;
    [Range(0, 10)]
    [SerializeField] private float _Density = 4.17f;

    [Range(0, 0.5f)]
    [SerializeField] private float _Weight = 0.06f;

    [Range(0, 1.5f)]
    [SerializeField] private float _Exposure = 0.71f;

   


  
    private void Update()
    {
#if DEBUG
        UpdateShader();
#endif
    }

    private void Start()
    {

        UpdateShader();
    }

    private void UpdateShader()
    {
        VLMaterial.SetFloat(VL_RadiusKeyword, _Radius);                      
        VLMaterial.SetFloat(VL_DecayKeyword, _Decay);                      
        VLMaterial.SetFloat(VL_DensityKeyword, _Density);                      
        VLMaterial.SetFloat(VL_WeightKeyword, _Weight);                      
        VLMaterial.SetFloat(VL_ExposureKeyword, _Exposure);

    }
}

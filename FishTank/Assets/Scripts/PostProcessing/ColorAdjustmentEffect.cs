using UnityEngine;

[ExecuteAlways]
public class ColorAdjustmentEffect : MonoBehaviour
{
    private const string BrightnessKeyword = "_Brightness";
    private const string ContrastKeyword = "_Contrast";
    private const string SaturationKeyword = "_Saturation";

    [SerializeField] private Material _colorAdjustmentMaterial = default;
    [ Range(-1,1)] public float _brightness = 1f;
    [ Range(0,2)] public float _contrast = 1f;
    [ Range(0,2)] public float _saturation = 1f;

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (_colorAdjustmentMaterial == null)
        {
            return;
        }

        _colorAdjustmentMaterial.SetFloat(BrightnessKeyword, _brightness);
        _colorAdjustmentMaterial.SetFloat(ContrastKeyword, _contrast);
        _colorAdjustmentMaterial.SetFloat(SaturationKeyword, _saturation);

        Graphics.Blit(src, dst, _colorAdjustmentMaterial);
    }

  
}

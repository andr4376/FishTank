using UnityEngine;

[ExecuteAlways]
public class VignetteEffect : MonoBehaviour
{
    private const string ColorKeyword = "_Color";
    private const string SizeKeyword = "_Size";
    private const string StrengthKeyword = "_Strength";

    [SerializeField] private Material _vignetteMaterial = default;
    [SerializeField] private Color _color = Color.black;
    [ Range(0,1)] public float _strength = 1;
    [ Range(0,1)] public float _size = 1;

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (_vignetteMaterial == null)
        {
            return;
        }

        _vignetteMaterial.SetFloat(SizeKeyword, _size);
        _vignetteMaterial.SetFloat(StrengthKeyword, _strength);
        _vignetteMaterial.SetColor(ColorKeyword, _color);

        Graphics.Blit(src, dst, _vignetteMaterial);
    }
}

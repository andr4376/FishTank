using UnityEngine;

public class VolumetricLightScatteringEffect : MonoBehaviour
{
	[SerializeField] private Material _lightScatteringMaterial = default;
	[SerializeField] private VolumetricLightScatteringCamera _prepassCamera = default;
	[SerializeField] private Light _light = default;

	private Camera _camera;

	private void Start()
	{
		_camera = GetComponent<Camera>();

        if (SaveManager.ValidSave)
        {
            this.enabled = SaveManager.Settings.volumetricLighting;
        }
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		Vector3 lightPosScreen = _camera.WorldToScreenPoint(_light.transform.position);
		_lightScatteringMaterial.SetTexture("_OcclusionPrepass", _prepassCamera.prepassTexture);
		_lightScatteringMaterial.SetFloat("_LightPosX", lightPosScreen.x);
		_lightScatteringMaterial.SetFloat("_LightPosY", lightPosScreen.y);
		Graphics.Blit(src, dst, _lightScatteringMaterial);
	}
}

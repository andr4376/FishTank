using UnityEngine;

[RequireComponent(typeof(Camera))]
public class VolumetricLightScatteringCamera : MonoBehaviour
{
	[SerializeField] private Camera _camera;
	[SerializeField] private Shader _volumetricScatteringPrepassShader;

	private RenderTexture _prepassTexture;
	public RenderTexture prepassTexture => _prepassTexture;

	private void Reset()
	{
		_camera = GetComponent<Camera>();
	}

	private void Awake()
	{
		_camera.enabled = true;
		_prepassTexture = new RenderTexture(Screen.width, Screen.height, 0);
		_prepassTexture.name = "Volumetric Scattering Prepass";
		_camera.targetTexture = _prepassTexture;
		_camera.SetReplacementShader(_volumetricScatteringPrepassShader, "");
	}
}

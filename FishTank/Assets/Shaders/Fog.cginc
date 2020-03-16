#include "UnityCG.cginc"

uniform float4 _FogColor;
uniform float _FogStart;
uniform float _FogEnd;
uniform float _HeightFogStart;
uniform float _HeightFogEnd;
uniform float _HeaightFogStrenght;
uniform float _FogIntensity;

float4 ApplyFog(float4 color, float3 worldPos)
{
	//Calculate world distance to camera
	float cameraDist = length(_WorldSpaceCameraPos - worldPos);

	//Convert distance to the fog range, and interpolate
	float fog = (cameraDist - _FogStart) / (_FogEnd - _FogStart);
	fog = smoothstep(0, 1, fog);

	//Convert height to the fog range, and interpolate
	float heightFog = (worldPos.y - _HeightFogStart) /
	 (_HeightFogEnd - _HeightFogStart);
	 
	heightFog = 1 - smoothstep(0, _HeaightFogStrenght, heightFog);

	float totalFog = max(fog, heightFog)*_FogIntensity;

	return  lerp(color, _FogColor, totalFog);
}



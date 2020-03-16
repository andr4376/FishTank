Shader "VolumetricLighting/VolumetricLightScattering"
{
    Properties
    {
		_MainTex("Texture", 2D) = "white"{}
		_Radius("Radius", Range(0,5)) = 0.5
		_Decay("Decay", Range(0,1.5)) = 1
		_Density("Density", Range(0.5,10)) = 1
		_Weight("Weight", Float) = 1
		_Exposure("Exposure", Float) = 1
    }
    SubShader
    {
        Cull Off
		ZWrite Off 
		ZTest Always
		
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"

			//default 128
			#define SAMPLES 40

            sampler2D _OcclusionPrepass;
            sampler2D _MainTex;
			float _Radius;
			float _Decay;
			float _Density;
			float _Weight;
			float _Exposure;

			float _LightPosX;
			float _LightPosY;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }


			half N21(half2 p)
			{
				p = frac(p*half2(225.54, 221.73));
				p += dot(p, p + 121.25);
				return frac(p.x * p.y);
			}

            fixed4 frag (v2f i) : SV_Target
            { 

				float2 uv = i.uv;
				// Calculate vector from pixel to light source in screen space.    
				float2 deltaUV = uv - float2(_LightPosX, _LightPosY) / _ScreenParams.xy;

				float lightDist = length(float2(deltaUV.x * 
				(_ScreenParams.x / _ScreenParams.y), deltaUV.y));
				
				// Divide by number of samples and scale by control factor.   
				deltaUV /= SAMPLES * _Density;

				// Store initial sample.    
				float3 color = tex2D(_MainTex , uv);
				// Set up illumination decay factor.    
				float illuminationDecay = 1.0f;
				float light = 0;
    
				for (int i = 0; i < SAMPLES; i++)   
				{     
					// Step sample location along ray.     
					uv -= deltaUV + (deltaUV * N21(uv));
					// Retrieve sample at new location.    
					float3 sampleCol = tex2D(_OcclusionPrepass, uv);
					// Apply sample attenuation scale/decay factors.     
					sampleCol *= illuminationDecay * _Weight;    
					// Accumulate combined color.     
					light += Luminance(sampleCol);
					// Update exponential decay factor.     
					illuminationDecay *= _Decay;   
				}
			
				light = lerp(light, 0, lightDist / _Radius);
				color += saturate(light  * _LightColor0.rgb);
				//Output final color with a further scale control factor.    

				return fixed4(color * _Exposure, 1);
		
            }
            ENDCG
        }
    }
}

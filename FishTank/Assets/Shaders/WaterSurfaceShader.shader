Shader "FishTank/WaterSurface"
{
	Properties
	{
		[Header(TEXTURE)]
		[Space(10)]
        _MainTex("Main Texture",2D) = "white"{}
        _TextureTransparency("TextureTransparency",Range(0,1)) = 1
        _ScrollSpeed("texture Scroll",Vector) = (0,0,0,0)
		[Space(10)]
		_Color("Color", Color) = (0,0,0,0)
		_ColorIntensity("Color Intensity", Range(0,1)) = 0.5
		[Space(10)]

        _NoiseTexture("NoiseTexture",2D) = "black"{}
        _NoiseScrollSpeed("Noise Scroll",Vector) = (0,0,0,0)
        _NoiseTransparency("Noise Transparency",Range(0,1)) = 1

		_Distortion ("Distortion", Range(1, 20)) = 1



	}
		SubShader
		
	{
		Tags {	"RenderType" = "Opaque" 
				}

		Pass
		{
			Cull Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag


			

			#include "UnityCG.cginc"
			#include "Assets/Shaders/Fog.cginc"
			#include "Assets/Shaders/FishTankLighting.cginc"
			
			struct appdata
			{
				float4 vertex : POSITION;
				float3 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 worldPos : TEXCOORD0;
				float2 uv : TEXCOORD1;

			
			};



            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _TextureTransparency;

			float4 _Color;
			float _ColorIntensity;

			sampler2D _NoiseTexture;
			float4 		_NoiseTexture_ST;
			float _NoiseTransparency;
			float3 _ScrollSpeed;
			float3 _NoiseScrollSpeed;
			float _Distortion;

	

			v2f vert(appdata v)
			{
				v2f o;

				//Fragments position in clip space
				o.pos = UnityObjectToClipPos(v.vertex);

				//WORLD SPACE
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
					
				//main texture coordinate
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);


				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{

			float4 color;					

			 half2 distortionOffset = i.uv;

			//noise sample for waving effect	
             half noiseVal = tex2D(_NoiseTexture, i.uv).r;

			//create a uv offset that makes a wavy texture distortion  effect
             distortionOffset = distortionOffset + noiseVal * sin(_Time.y * _NoiseScrollSpeed.xy) / _Distortion;
					 
			//get the color of the distorted main texture with texture scrolling 
			float4 texColor = tex2D(_MainTex, (i.uv +
           		 _ScrollSpeed.xy * _Time.yy)+distortionOffset) * _TextureTransparency;

			//Apply color additively (texture is black with white stripes, abd this removes the black colors)
			color = _Color + texColor;
			
			//Apply fog from the Fog.cginc file
			color= ApplyFog(color, i.worldPos);

				return color;

			}
			ENDCG
	}
}
		//allow shadow castiong from standard shaders
				Fallback "Diffuse"
}

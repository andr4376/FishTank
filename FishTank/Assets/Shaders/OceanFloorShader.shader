Shader "FishTank/OceanFloor"
{
	Properties
	{
		[Header(TEXTURE)]
		[Space(10)]
        _MainTex("Main Texture",2D) = "white"{}
        _TextureTransparency("TextureTransparency",Range(0,1)) = 1
		[Space(10)]
		_Color("Color", Color) = (0,0,0,0)
		_ColorIntensity("Color Intensity", Range(0,1)) = 0.5
		[Space(10)]

        _NoiseTexture("NoiseTexture",2D) = "black"{}
        _ScrollSpeed("Noise Scroll",Vector) = (0,0,0,0)
        _NoiseTransparency("Noise Transparency",Range(0,1)) = 1
        _NoiseShadowThreshold("Noise Shadow Threshold",Range(0,1)) = 1

		

		[Space(15)]
		[Header(Light Settings)]
        [Space(10)]
        [Header(Light colors)]
        [Space(5)]
        _AmbientColor("Ambient Color (Shadow color)",Color)=(1,1,1,1)
        _SpecularLightColor("Specular Light Color", Color)= (1,1,1,1)
        _RimColor("Rim Color",Color)=(0,0,0,0)

        [Header(Specular light settings)]
        [Space(5)]
        _Glosiness("Gloss",Float)=1
        _SpecularBlur("Specular Blur", Float) = 0

        [Header(Rim light Settings)]
        [Space(5)]
        _RimThreshold("Rim Threshold",Range(0,1))=0.1
        _RimAmount("Rim Amount",Range(0,1))=0.5


	}
		SubShader
		
	{
		Tags {	"RenderType" = "Opaque" 
				"LightMode" = "ForwardBase"
				"PassFlags" = "OnlyDirectional"}

		Pass
		{
			Cull Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag


			//OBS: Include if using FishTankLighting.cginc!!
            //Compiles multiple versions based on light settings
            //Needed in order to  apply shadow from other shadow casters 
			#pragma multi_compile_fwdbase

			#include "UnityCG.cginc"
			#include "Assets/Shaders/Fog.cginc"
			#include "Assets/Shaders/FishTankLighting.cginc"
			
			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float3 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 worldPos : TEXCOORD0;
				float2 uv : TEXCOORD1;

				//Light
				float3 worldNormal : NORMAL;
				float3 viewDir : TEXCOORD2;	
				SHADOW_COORDS(4)

				float2 noiseUV : TEXCOORD3;
			};



            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _TextureTransparency;

			float4 _Color;
			float _ColorIntensity;

			sampler2D _NoiseTexture;
			float4 _NoiseTexture_ST;
			float _NoiseTransparency;
			float3 _ScrollSpeed;
			float _NoiseShadowThreshold;



			//Shadow / light colors
            float4 _AmbientColor;
            float4 _SpecularLightColor;
            float4 _RimColor;

            //How shiny should the object appear
            float _Glosiness;
            //Defines the sharpness of the specular glow 
            float _SpecularBlur;

            //Rim light settings
            float _RimThreshold;
            float _RimAmount;

			v2f vert(appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);

				//WORLD SPACE
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				
				//Get normal in world space
				o.worldNormal = UnityObjectToWorldNormal(v.normal);

				//View Direction in world space
				o.viewDir = WorldSpaceViewDir(v.vertex);

				//main texture coordinate
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				//shadow
				TRANSFER_SHADOW(o)
                o.noiseUV = TRANSFORM_TEX(v.uv, _NoiseTexture);

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{

	float4 color;

	/*
	Use FishTankLighting.cginc file to calculate 
	Blinn-phong lighting + Rim lighting	
	*/
	float4 light =
               GetLight(      
                    i.worldNormal,
                    i.viewDir,
                    SHADOW_ATTENUATION(i),
					TOONY_SHADOW_EDGE, //defines in my light include
                    _Glosiness,
                    _SpecularLightColor,
                    _SpecularBlur,
                    _RimThreshold,
                    _RimAmount,
                    _RimColor);		
					



			//Noise 
			//Sample noise
            float noise = tex2D(_NoiseTexture, i.noiseUV +
           		 _ScrollSpeed.xy * _Time.yy).r;		


				 

			float4 texColor = tex2D(_MainTex, i.uv) * _TextureTransparency;


			color = (light + _AmbientColor) * texColor +
	 				(_Color * _ColorIntensity);

			noise*= color.rgb>_NoiseShadowThreshold
			 		|| _NoiseShadowThreshold==0; 

			color+=  (noise*_NoiseTransparency);
				color= ApplyFog(color, i.worldPos);

				return color;

			}
			ENDCG
	}
}
		//allow shadow castiong from standard shaders
				Fallback "Diffuse"
}

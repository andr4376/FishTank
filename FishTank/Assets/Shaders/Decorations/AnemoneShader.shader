﻿Shader "FishTank/AnemoneShader"

{
	Properties
	{
		[Header(TEXTURE)]
		[Space(10)]
        _MainTex("Main Texture",2D) = "white"{}
        _TextureTransparency("TextureTransparency",Range(0,1)) = 1
		_Color("Color", Color) = (0,0,0,0)
		_ColorIntensity("Color Intensity", Range(0,1)) = 0.5

        _Speed("Speed", Float) = 1
		_Frequency("_Frequency", Float) = 1
		_Amplitude("Amplitude", Float) = 1
		
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

		////////////Ocean Floor
		[Space(15)]
		[Header(Ocean Floot Settings)]
        [Space(10)]
		_OceanFloorTransparency("Ocean floor light transparency", Range(0,1)) = 1


	}
		SubShader
	{

		Tags {	"RenderType" = "Opaque" 
			"LightMode" = "ForwardBase"
			"PassFlags" = "OnlyDirectional"}

		Cull Off
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase

			#include "UnityCG.cginc"
			#include "Assets/Shaders/Fog.cginc"
			#include "Assets/Shaders/FishTankLighting.cginc"
			#include "Assets/Shaders/OceanFloorLight.cginc"

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
				SHADOW_COORDS(3)
			};



            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _TextureTransparency;
 			float4 _Color;
			float _ColorIntensity;

            float _Speed;
			float _Frequency;
			float _Amplitude;


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

			//ocean floo
			float _OceanFloorTransparency;

			v2f vert(appdata v)
			{
				v2f o;

                //Anemone movement

                float wave = sin((
				v.vertex.xyz
				+ _Time.x*_Speed)*_Frequency) *_Amplitude;
			wave *= (- v.vertex.y);


                v.vertex.xz += wave;

                //  

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
					NATURAL_SHADOW_EDGE, //defines in my light include
                    _Glosiness,
                    _SpecularLightColor,
                    _SpecularBlur,
                    _RimThreshold,
                    _RimAmount,
                    _RimColor);		
					
    
	
	float4 texColor = tex2D(_MainTex, i.uv);

	color = (light + _AmbientColor)
	 * (texColor*_TextureTransparency) 
	 *(_Color * _ColorIntensity);

	// apply oceanLight (from OceanFloorLight.cginc)
	 color += GetOceanLight
		            (i.worldNormal,
		            i.worldPos,
		            SHADOW_ATTENUATION(i),
		            _OceanFloorTransparency);		

	color= ApplyFog(color, i.worldPos);

	return color;

		}
			ENDCG
	}
	Pass

		{
		Tags {	"LightMode" = "ShadowCaster" }

		Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster

			#include "UnityCG.cginc"

		

	//Fragment
	//vertext to fragment
		struct v2f
		{
			V2F_SHADOW_CASTER;
		};

	

		
	float _Speed;
			float _Frequency;
			float _Amplitude;

		v2f vert(appdata_base v)
		{
			v2f o;

                //Anemone movement

                float wave = sin((
				v.vertex.xyz
				+ _Time.x*_Speed)*_Frequency) *_Amplitude;
			wave *= (- v.vertex.y);


                v.vertex.xz += wave;

                //  

				o.pos = UnityObjectToClipPos(v.vertex);
		return o;
		}
		
		fixed4 frag(v2f i) : SV_Target
		{			
				SHADOW_CASTER_FRAGMENT(1)
		
		}
		ENDCG
	}
}
		
}

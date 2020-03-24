Shader "FishTank/FishShader" {
	Properties{
		
		_MainTex("Fish texture", 2D) = "white" {}
		_Color("Fish Color", Color) = (0,0,0,0)
		_ColorIntensity("Color Intensity", Range(0,1)) = 0.5

		[Header(Fish Motion Settings)]
		[Space(10)]			
		_TailSpeed("Tail Speed", Range(0, 300)) = 1
		_TailFrequency("Tail Frequency", Range(0, 15)) = 1
		_TailAmplitude("Tail Wiggle Range", Range(0,  3)) = 1
		_HeadMovement("Head Movement", Range(0,  1)) = 0.05
		_HeadStart("Head Start", Range(-10,  10)) = 0.05

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
SubShader{
		Tags{ "RenderType" = "Opaque" 
		"LightMode" = "ForwardBase"
		"PassFlags" = "OnlyDirectional"}
		
		Cull Off

	Pass{

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
		float2 uv : TEXCOORD;
		float3 normal : NORMAL;
	};

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;

		//fog	
		float3 worldPos : TEXCOORD1;

		//Light
		float3 worldNormal : NORMAL;
		float3 viewDir : TEXCOORD2;	
		SHADOW_COORDS(3)

	};

	
	sampler2D _MainTex;
	float4 _MainTex_ST;
	float4 _Color;
	float _ColorIntensity;

	float _TailSpeed;
	float _TailFrequency;
	float _TailAmplitude;
	float _HeadStart;
	float _HeadMovement;

	

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


			//ocean floor
			float _OceanFloorTransparency;

	//Manipulate the positions of the input verticies to 
	//fit the motion of a fish
	v2f vert(appdata v)
	{
		
		v2f o;
		

		// if (v.vertex.x> _HeadStart)
		//(The start of the "spine")
		// Move based on head movement factor
		v.vertex.z +=		 
		 (sin(
			 ( _Time.x * _TailSpeed)*_TailFrequency)
			 *_TailAmplitude *
			_HeadMovement)//Movement Calc End
			* 
			(v.vertex.x > _HeadStart);//1 if true 0 if false

		//
		//ELSE:
		// Move based on X pos (the end of the tail moves more)
				
		v.vertex.z +=	 
			(sin(
			(v.vertex.x-_HeadStart + _Time.x * _TailSpeed)*_TailFrequency)
			*_TailAmplitude * 
			(v.vertex.x-_HeadStart))//Intensify wave movement further from the head
			 //Movement Calc End
			* 
			(v.vertex.x < _HeadStart);//1 if true 0 if false
		//
		
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = TRANSFORM_TEX(v.uv, _MainTex);

		//WORLD SPACE
		o.worldPos = mul(unity_ObjectToWorld, v.vertex);

		//Get normal
		o.worldNormal = UnityObjectToWorldNormal(v.normal);

		//View Direction
		o.viewDir = WorldSpaceViewDir(v.vertex);

		TRANSFER_SHADOW(o)
		
		return o;

	}

	//Foreach pixel
	fixed4 frag(v2f i) : SV_Target
	{
	
	float4 color;

	/*
	Use FishTankLighting.cginc file to calculate 
	Blinn-phong lighting + Rim lighting	
	*/
	float4 light =
        GetLight(      
        i.worldNormal, 			//Normal in world space
        i.viewDir,				//World space view direction
        SHADOW_ATTENUATION(i),	//Macro that tells if the fragmet is in other's shadows
		TOONY_SHADOW_EDGE, 		//defines in my light include
        _Glosiness,				//Gloss / Shine / Highlight
        _SpecularLightColor,	//Specular light color
        _SpecularBlur,			//Specular Blur
        _RimThreshold,			//Rim Threshold
        _RimAmount,				//Rim Amount
        _RimColor);				//Rim Color
					

	float4 texColor = tex2D(_MainTex, i.uv);

	color = (light + _AmbientColor) * texColor +
	 		(_Color * _ColorIntensity);

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

	//SHADOW PASS
	//In order to draw shadows, we do all the calculations one more time				
	//But instead of drawing the fish, we use SHADOW_CASTER_FRAGMENT
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

	

		
	float _TailSpeed;
	float _TailFrequency;
	float _TailAmplitude;
	float _HeadStart;

	float _HeadMovement;

		v2f vert(appdata_base v)
		{
			v2f o;
		
		// if (v.vertex.x> _HeadStart)
		//(The start of the "spine")
		// Move based on head movement factor
		v.vertex.z +=		 
		 (sin(
			 ( _Time.x * _TailSpeed)*_TailFrequency)
			 *_TailAmplitude *
			_HeadMovement)//Movement Calc End
			* 
			(v.vertex.x > _HeadStart);//1 if true 0 if false

		//
		//ELSE:
		// Move based on X pos (the end of the tail moves more)
				
		v.vertex.z +=	 
			(sin(
			(v.vertex.x-_HeadStart + _Time.x * _TailSpeed)*_TailFrequency)
			*_TailAmplitude * 
			(v.vertex.x-_HeadStart))//Intensify wave movement further from the head
			 //Movement Calc End
			* 
			(v.vertex.x < _HeadStart);//1 if true 0 if false
	
		
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
		//	FallBack "Diffuse"	//allows casting of shadows


}

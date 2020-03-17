Shader "FishTank/FishShader" {
	Properties{
		
		_MainTex("Fish texture", 2D) = "white" {}

		[Header(Fish Motion Settings)]
		[Space(10)]			
		_TailSpeed("Tail Speed", Range(0, 300)) = 1
		_TailFrequency("Tail Frequency", Range(0, 15)) = 1
		_TailAmplitude("Tail Wiggle Range", Range(0,  3)) = 1
		_HeadMovement("Head Movement", Range(0,  1)) = 0.05
		_HeadStart("Head Start", Range(-10,  10)) = 0.05

		[Header(COLOR)]
		[Space(10)]
	_AmbientColor("Ambient Color", Color) = (1,1,1,1)
		[Space(15)]
		[Header(LIGHT SETTINGS)]
		[Space(10)]
		[Header(Specular Lighting)]
		[Space(10)]

	_SpecularPower("Specular Power", Range(0,100))=47
	_SpecularStrenght("Specular strenght", Range(0,1))=0.5
		[Space(15)]
		[Header(Rim Lighting)]
		[Space(10)]

	_RimColor("Rom Color",Color) = (1,1,1,1)
	_RimStrenght	("Rim Strenght", Range(0,1))=1
	_RimThreshold		("Rim Threshold", Range(0,1))=1
	_RimAmount		("Rim Amount", Range(0,1))=1

		[Space(15)]
		[Header(Shadow settings)]
		[Space(10)]

	_LightIntensity("Light Intensity",Range(-0.5,0.5)) = 0
	_ShadowSoftness("Shadow Softness", Range(0,0.02)) =0.01
	}
SubShader{
		Tags{ "RenderType" = "Opaque" 
		"LightMode" = "ForwardBase"}
		
		Cull Off

	Pass{

	CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#pragma multi_compile_fwdbase

	#include "UnityCG.cginc"
	#include "Assets/Shaders/Fog.cginc"
	#include "UnityLightingCommon.cginc"	
	#include "AutoLight.cginc"

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

		//shadow
		float3 viewDirection : TEXCOORD2;
		float3 normal : NORMAL;
		SHADOW_COORDS(3)

	};

	
	sampler2D _MainTex;
	float4 _MainTex_ST;

	float _TailSpeed;
	float _TailFrequency;
	float _TailAmplitude;
	float _HeadStart;
	float _HeadMovement;

	//Color Light and shadow
			float4 _AmbientColor;	
			float _SpecularPower;
			float _SpecularStrenght;//glow spot intensity

			float _ShadowSoftness;

			//Rim lighting
			float _RimStrenght;
			float _RimAmount;
			float4 _RimColor;
			float _RimThreshold;
			float _LightIntensity;

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
		o.normal = UnityObjectToWorldNormal(v.normal);

		//View Direction
		o.viewDirection = WorldSpaceViewDir(v.vertex);

		TRANSFER_SHADOW(o)
		
		return o;

	}

	//Foreach pixel
	fixed4 frag(v2f i) : SV_Target
	{
		//lighting
				float3 normal = normalize(i.normal);
				float3 viewDirection = normalize(i.viewDirection);

				//diffuse dot product
				//based on light direction and fragment normal
				float nDotL = dot(_WorldSpaceLightPos0, normal);

				//get the amount of light the fragment should have
				//from 0 - 1 (darkness and brightness)
 				float diffuse = max(0,nDotL) * SHADOW_ATTENUATION(i);

				//Make sure shadow is not darker than what the material allows
				diffuse = max(_ShadowSoftness,diffuse);

				//toon (Very sharp shadow edge)
				diffuse = smoothstep(0,0.02,diffuse); //adjust second val


				//Specular lighting
				float3 r = -reflect(_WorldSpaceLightPos0, normal);
				float rDotV = max(0,dot(r, viewDirection));
				float specular = pow(rDotV, _SpecularPower);

				//toon
				specular = smoothstep(0,0.2,specular)*_SpecularStrenght; 
				
				//Rim
				float rimDot = 1-dot(viewDirection,normal);
				float rimIntensity = rimDot * pow(nDotL,_RimThreshold );
				//sharp edges
						rimIntensity = smoothstep(_RimAmount-0.01,
						 _RimAmount+0.01, rimIntensity) * _RimStrenght;

				float4 rim = rimIntensity*_RimColor;

				
	
				float4 texColor = tex2D(_MainTex, i.uv) ;

				//Primary color is ambient color mixed with texture 
				float4 color =	(_AmbientColor * texColor); 

				//Rim, Specular and light color 
				float4 lightFactor = 
				((_LightColor0 * _LightIntensity) +
				  specular + rim);

				//apply ligthing
				 color *=
				 (diffuse+ lightFactor); //+ for high exposure * for darker

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
		//
		
		o.pos = UnityObjectToClipPos(v.vertex);
		return o;
		}
		///Tegner alle pixels for hver vertices
		fixed4 frag(v2f i) : SV_Target
		{			
				SHADOW_CASTER_FRAGMENT(1)
		
		}
		ENDCG
	}
	}
		//	FallBack "Diffuse"	//allows casting of shadows
}
Shader "FishTank/GlassShader"
{
    Properties
    {
        
		_GlassColor("Color", Color) = (1,1,1,1)
		_AmbientColor("Ambient Color", Color) = (1,1,1,1)

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
        Tags { "RenderType"="Transparent"
                "LightMode" = "ForwardBase"
			    "PassFlags" = "OnlyDirectional"
                "RenderQueue"="Transparent" }
        LOD 100
        

        Pass
        {

            Blend SrcAlpha OneMinusSrcAlpha //Traditional Transparency
            Cull Back
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma multi_compile_fwdbase
            #pragma Standard fullforwardshadows alpha

            #include "UnityCG.cginc"
			#include "Assets/Shaders/Fog.cginc"
            #include "Assets/Shaders/FishTankLighting.cginc"
			#include "Assets/Shaders/OceanFloorLight.cginc"


            struct appdata
            {
                float4 vertex : POSITION;
               	float3 normal : NORMAL;

				

            };

            struct v2f
            {
                float4 vertex : SV_POSITION;

                float3 worldPos : TEXCOORD0;

                   //Light
				float3 worldNormal : NORMAL;
				float3 viewDir : TEXCOORD2;	
				SHADOW_COORDS(3)
            };


            float4 _GlassColor;


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

            //Ocean floor
            float _OceanFloorTransparency;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                //WORLD SPACE
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				
				//Get normal in world space
				o.worldNormal = UnityObjectToWorldNormal(v.normal);

				//View Direction in world space
				o.viewDir = WorldSpaceViewDir(v.vertex);

				//TRANSFER_SHADOW(o)


                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = _GlassColor;

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

                col*= light +_AmbientColor;

            // apply oceanLight (from OceanFloorLight.cginc)
	        col += GetOceanLight
		            (i.worldNormal,
		            i.worldPos,
		            SHADOW_ATTENUATION(i),
		            _OceanFloorTransparency);
                
                return  ApplyFog(_GlassColor+col, i.worldPos);
            }
            ENDCG
        }
    }
				Fallback "Diffuse"

}

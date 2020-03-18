Shader "Unlit/unlitShaderTest"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}


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
        Tags { "RenderType"="Opaque"
                "LightMode" = "ForwardBase"
				"PassFlags" = "OnlyDirectional" }

        Pass
        {            
			
            CGPROGRAM
            #pragma vertex vert 
            #pragma fragment frag


            //OBS: Include if using FishTankLighting.cginc!!
            //Compiles multiple versions based on light settings
            //Needed in order to  apply shadow from other shadow casters            
			#pragma multi_compile_fwdbase

            #include "UnityCG.cginc"
           	#include "Assets/Shaders/FishTankLighting.cginc"


 
            struct appdata
            {
                float4 vertex : POSITION;				
				float4 uv : TEXCOORD0;
				float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 worldNormal : NORMAL;
				float3 viewDir : TEXCOORD1;	
				SHADOW_COORDS(2)

            };

            sampler2D _MainTex;
            float4 _MainTex_ST;


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

            v2f vert (appdata v)
            {
                v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);		
				o.viewDir = WorldSpaceViewDir(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				// Defined in Autolight.cginc. Assigns the above shadow coordinate
				// by transforming the vertex from world space to shadow-map space.
				TRANSFER_SHADOW(o)
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                
                
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



                return (light+ _AmbientColor ) * col;





            }

            ENDCG
        }
        
    }

    //shadow casting
    Fallback "Diffuse"

   
}

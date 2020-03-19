Shader "FishTank/SeaweedShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTexture ("_SeaWeedMask", 2D) = "white" {}

        _Speed("Speed", Float) = 1
		_Frequency("_Frequency", Float) = 1
		_Amplitude("Amplitude", Float) = 1
        _Wave("Wave",Vector) = (0,0,0,0)

        
        
		

    }
    SubShader
    {
        Tags { "RenderType"="Transparent" 
                "Render Queue" = "Transparent"
               }
        LOD 100

        Pass
        {

            Blend SrcAlpha OneMinusSrcAlpha //Traditional Transparency
			ZWrite Off //Don't write to depth buffer
            Cull Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
			#include "Assets/Shaders/Fog.cginc"
            #include "Lighting.cginc"


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal :NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
			    float3 worldPos : TEXCOORD1;

               

            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _NoiseTexture;
            float4 _NoiseTexture_ST;

            
            float _Speed;
			float _Frequency;
			float _Amplitude;

	        float3 _Wave;
            

            v2f vert (appdata v)
            {
                v2f o;
			    o.uv = v.uv;

			float waveMovement = sin((
				(v.vertex.x + v.vertex.y + v.vertex.z)
				+ _Time.x*_Speed)*_Frequency) *_Amplitude			;

           
            //Prevents the stem from waving
             waveMovement*= (v.vertex.x-0.5); //base movement on how far v is from the middle (x)   

			float4 animatedPosition = v.vertex +
             float4(v.normal,0) * waveMovement; //Movement is based on v normal

			o.worldPos = mul(unity_ObjectToWorld, animatedPosition);
			o.pos = UnityObjectToClipPos(animatedPosition);
           

			return o;

            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                
                col = ApplyFog(col*_LightColor0,i.worldPos);

                return col;
            }
            ENDCG
        }

    }
}

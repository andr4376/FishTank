Shader "PostProcessing/StaticEffectShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTexture("Noise Texture",2D) = "white"{}
		_ScrollSpeed("Scroll Speed", Vector) = (0.1,0.1,0,0)
		_Visibility("Visibility", Range(0,1)) = 0.3

    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 noiseUV: TEXCOORD1;
            };

           

            sampler2D _MainTex;
            sampler2D _NoiseTexture;
            float4 _NoiseTexture_ST;
            float4 _ScrollSpeed;
            float _Visibility;

             v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.noiseUV = TRANSFORM_TEX(v.uv, _NoiseTexture);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
              
              //Sample noise
                float noise = tex2D
                (_NoiseTexture, i.noiseUV + _ScrollSpeed.xy * _Time.yy).r;

                return col+(noise * _Visibility);
            }
            ENDCG
        }
    }
}

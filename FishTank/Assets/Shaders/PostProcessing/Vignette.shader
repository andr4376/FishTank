Shader "PostProcessing/Vignette"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (0,0,0,0)
		_Strength ("Strength", Range(0,1)) = 1
		_Size("Size", Range(0,1)) = 1

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
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
			float4 _Color;
			float _Strength;
			float _Size;

            fixed4 frag (v2f i) : SV_Target
            {
				//Read the screen
                fixed4 col = tex2D(_MainTex, i.uv);

				float2 uv = abs(i.uv - 0.5);
				float dist = length(uv);
				float vignette = saturate(dist / (1 - _Size)) * _Strength;

                return lerp(col, _Color, vignette);
            }
            ENDCG
        }
    }
}

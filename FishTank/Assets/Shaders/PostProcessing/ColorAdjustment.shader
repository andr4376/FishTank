Shader "PostProcessing/Color Adjustment"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Saturation ("Saturation", Range(0,2)) = 1
		_Brightness ("Brightness", Range(-1,1)) = 1
		_Contrast("Contrast", Range(0,2)) = 1
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
			float _Saturation;
			float _Brightness;
			float _Contrast;

            fixed4 frag (v2f i) : SV_Target
            {
				//Read the screen
                fixed4 col = tex2D(_MainTex, i.uv);
				
                //Contrast
				col.rgb = (col.rgb - 0.5) * _Contrast + 0.5;
				
                //Brigtness
                col.rgb += _Brightness;

                //luminance factor for saturation
				float luminance = (col.r * .3 + col.g * .59 + col.b * .11);
                //lerp saturation
				col.rgb = lerp(luminance, col.rgb, _Saturation);
                return col;
            }
            ENDCG
        }
    }
}

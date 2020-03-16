Shader "FishTank/WaterShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TextureOpacity("Texture Opacity", Range(0,1))=1
		_GlassColor("Color", Color) = (1,1,1,1)

    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _TextureOpacity;

            float4 _GlassColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                col *= _TextureOpacity;
                
                return _GlassColor+col;
            }
            ENDCG
        }
    }
}

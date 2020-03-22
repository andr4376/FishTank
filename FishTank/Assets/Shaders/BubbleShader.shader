Shader "FishTank/BubbleShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TextureTransparency("Texture Transparency", Range(0,1) ) = 0.5
        _BubbleColor("Ambient Color", Color) = (1,1,1,1)

    }
    SubShader
    {
        Tags { "RenderType"="Transparent"
                 }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha //Traditional Transparency
            Cull Back

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            

            #include "UnityCG.cginc"
            #include "Assets/Shaders/Fog.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _BubbleColor;
            float _TextureTransparency;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                o.worldPos = mul(unity_ObjectToWorld, v.vertex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) * _TextureTransparency;

                col *= _BubbleColor;

                col = ApplyFog(col, i.worldPos);
                return col;
            }
            ENDCG
        }
    }
}

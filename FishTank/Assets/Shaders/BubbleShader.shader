Shader "FishTank/BubbleShader"
{
	/*
	A simple transparent shader that are meant to draw 2d billbord bubbles
	for the particle systems
	*/
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
				//Clip position
                o.vertex = UnityObjectToClipPos(v.vertex);

				//Uv for bubble texture
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				//World position for fog
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) * _TextureTransparency;
				
				//the sprites are white, so i can apply color multiplicatively
                col *= _BubbleColor;

				//apply fog from my fog.cginc file
                col = ApplyFog(col, i.worldPos);
                return col;
            }
            ENDCG
        }
    }
}

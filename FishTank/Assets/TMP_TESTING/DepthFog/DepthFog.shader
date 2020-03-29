Shader "PostProcessing/DepthFog"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
                float2 uv : TEXCOORD1;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 screenPosition:TEXCOORD0;
                float2 uv : TEXCOORD1;

            };

            sampler2D _CameraDepthTexture;
            float4 _FogColor;

            float _FogStart;
            float _EndOfClipSpace;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPosition = ComputeScreenPos(o.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                //sample the depth buffer with screen position
                float depth =
                 Linear01Depth(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(i.screenPosition)).r);
                                
                float fog = (depth - _FogStart) / (_EndOfClipSpace - _FogStart);
                fog = smoothstep(0,1,fog);

                float4 pixelColor = tex2Dproj(_MainTex,i.screenPosition);

                return lerp(pixelColor,_FogColor,depth);
            }
            ENDCG
        }
    }
}

Shader "Unlit/LiquidShader"
{
	
    Properties
    {
        _LiquidColor ("Tint", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _FillAmount ("Fill Amount", Range(-10,10)) = 0.0   
        _FoamTopColor ("Top Color", Color) = (1,1,1,1)
        _FoamSideColor ("Foam Side Color", Color) = (1,1,1,1)
        _FoamHeight ("Foam Width", Range(0,0.75)) = 0.0    
    }
 
    SubShader
    {
        Tags {"Queue"="Geometry"  "DisableBatching" = "True" }
 
                Pass
        {
         Zwrite On
         Cull Off // Draw the backface too
         AlphaToMask On // transparency
 
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
            float liquidSurface : TEXCOORD2;
		      	float3 worldPos : TEXCOORD3;

         };
 
         sampler2D _MainTex;
         float4 _MainTex_ST;
         float _FillAmount;
         float4 _FoamTopColor, _FoamSideColor, _LiquidColor;
         float _FoamHeight;
           
        
     
 
         v2f vert (appdata v)
         {
            v2f o;
 
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = TRANSFORM_TEX(v.uv, _MainTex);
            // get world position of the vertex
            o.worldPos= mul (unity_ObjectToWorld, v.vertex.xyz);  
            
            // how high up the liquid is
            o.liquidSurface =  o.worldPos.y + _FillAmount;
 
            return o;
         }
           
         fixed4 frag (v2f i, fixed facing : VFACE) : SV_Target
         {
           // sample the texture and color
           fixed4 liquidColor = tex2D(_MainTex, i.uv) * _LiquidColor;
           
           // foam line on the side of glass
           //if this pizel is between the bottom of the foam line,
           //and the top of the foam line, 1, else 0
           float4 sideFoam =
            (step(i.liquidSurface, 0.5)
             - 
             step(i.liquidSurface,(0.5 - _FoamHeight)));

           float4 foamColor = sideFoam * _FoamSideColor;

           // rest of the liquid
           float4 liquid = step(i.liquidSurface, 0.5) - sideFoam;

           float4 resultColored = liquid * liquidColor;
           // both together, with the texture
           float4 finalResult = resultColored + foamColor;               
 
           // color of backfaces/ top
           float4 topColor = _FoamTopColor * (sideFoam + liquid);
           
           //VFACE returns positive for front facing, negative for backfacing
           //Gives the illusion of foam on top by collering the backface
           //of the hollow liquid the top color
           float4 color = facing > 0 ? finalResult  : topColor;

           return ApplyFog(color, i.worldPos);
               
         }
         ENDCG
        }
 
    }
    Fallback "Diffuse"
}
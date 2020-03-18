//Tags
//Add these tags in the start of a Pass:
/*
			Tags
			{
                //Use forward rendering 
				"LightMode" = "ForwardBase"

                //Only receive light data from directional light
				"PassFlags" = "OnlyDirectional"
			}	

*/		
// Setup our pass to use Forward rendering, and only receive
// data on the main directional light and ambient light.


//Add this Pragma underneath frag and vert: 
// #pragma multi_compile_fwdbase
// It is needed in order to receive shadow data from other objects

#include "UnityCG.cginc"

//Unity functionality that assists with shadows and lighting
#include "Lighting.cginc"
#include "AutoLight.cginc"
//COPY:
//#include "Assets/Shaders/FishTankLighting.cginc"


#define TOONY_SHADOW_EDGE 0.02
#define NATURAL_SHADOW_EDGE 0.4

//
float4 LambertianDiffuse(      
     float3 worldSpaceNormal,
      float shadow,
      float shadowEdgeBlur
       )
{
    
	float lightDotNormal =max(dot(_WorldSpaceLightPos0,
                             normalize(worldSpaceNormal)),0);			
				
	float lightIntensity
             = smoothstep(0,shadowEdgeBlur, lightDotNormal * shadow);	

	float4 light = lightIntensity * _LightColor0;
	
    
    return  light;
}

float4 SpecularLighting(      
     float3 worldSpaceNormal,
      float3 viewDirection,
      float shadow,
      float glossiness,
      float4 specularColor,
      float specularBlur = 0)
{

    float4 specular;
    
	float NdotL = dot(_WorldSpaceLightPos0, normalize(worldSpaceNormal));		
	float lightIntensity = smoothstep(0,0.01, NdotL * shadow);	

	float3 halfVector = normalize(_WorldSpaceLightPos0 + normalize(viewDirection));
	float NdotH = dot(normalize(worldSpaceNormal), halfVector);

	// Multiply _Glossiness by itself to allow artist to use smaller
	// glossiness values in the inspector.
	float specularIntensity = pow
	(NdotH * lightIntensity, glossiness);
	float specularIntensitySmooth =
     smoothstep(0.005, 0.01+(specularBlur*0.01), specularIntensity);

	 specular = specularIntensitySmooth * specularColor;	

    return specular;
}

//Combine the Lambertian lighting model with a Specular light
float4 BlinnPhong(     
     float3 worldSpaceNormal,
      float3 viewDirection,
      float shadow,
      float shadowEdgeBlur,
      float glossiness,
      float4 specularColor,
      float specularBlur = 0)
{

        float4 lambertian = LambertianDiffuse(worldSpaceNormal,shadow,shadowEdgeBlur);
        
        float specular = SpecularLighting(worldSpaceNormal,
        viewDirection,shadow, glossiness,specularColor,specularBlur);


        return lambertian + specular;
}

float4 RimLighting(
     float3 worldSpaceNormal,
      float3 viewDirection,
      float rimThreshold,
      float rimAmount,
      float4 rimColor)
{

	float NdotL = dot(_WorldSpaceLightPos0, normalize(worldSpaceNormal));		

				float rimDot = 1 - dot(
                    normalize(viewDirection)
                    ,normalize(worldSpaceNormal));

				// We only want rim to appear on the lit side of the surface,
				// so multiply it by NdotL, raised to a power to smoothly blend it.
				float rimIntensity = rimDot * pow(NdotL, rimThreshold);
				rimIntensity = smoothstep(rimAmount - 0.01,
                 rimAmount + 0.01, rimIntensity);
				float4 rim = rimIntensity * rimColor;

                return rim;
}

//Uses BlinnPhong + Rim lighting
float4 GetLight(
      float3 worldSpaceNormal,  //Where is the fragment in world space
      float3 viewDirection,     //World space camera direction  
      float shadow,             //whether or not the object is already in the shadow  
      float shadowEdgeBlur,     //Sharpness of the Lamberdian diffuse  
      float glossiness,         //The amount of specular lighting
      float4 specularColor,     //Color of the specular light 
      float specularBlur,       //Sharpness of the specular light spot
      float rimThreshold,       //Rim threshold
      float rimAmount,          //Amount of Rim lighting
      float4 rimColor           //Color of the rim light
      )
      
{

          float4 blinnPhong = BlinnPhong(
              worldSpaceNormal,
              viewDirection,
              shadow,
              shadowEdgeBlur,
              glossiness,
              specularColor,
              specularBlur              
          );

          float4 rim = RimLighting(worldSpaceNormal,viewDirection,
          rimThreshold,
              rimAmount,
              rimColor);

        return blinnPhong + rim;

}


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
     float3 worldSpaceNormal,   //fragments normal vector in world space
      float shadow,             //Whether or not the object is already in the shadows (From unity's functionality)
      float shadowEdgeBlur      //the sharpness of the line between light and shadow on the model
       )
{
    //1)
    //compare the similarity of the direction of the normal to the direction towards the directional light
    //by getting the DOT product.
    //Both vectors should be normalized
    //if the two vectors are the same, the output is 1, if the directions are oposite, the outcome is -1
    //if the two vectors make a Right Angle, the outcome is 0.

    /*  float dot(float4 a, float4 b)
    {
        return a.x*b.x + a.y*b.y + a.z*b.z + a.w*b.w;
    }
    */
    /* (y represents how much light should be applied):  
                    y    
            1       ^   (lit)
            0.75    |   (lit)
            0.5     |   (lit)
            0.25    |   (lit)
          <--0------|---(unlit)---> x
            -0.25   |   (unlit)
            -0.5    |   (unlit)
            -0.75   |   (unlit)
            -1      v   (unlit)
    */

    //Get dot product of the two directions, but limit darkness amount to 0
	float lightDotNormal =max(dot
                            (normalize(_WorldSpaceLightPos0),
                             normalize(worldSpaceNormal)),0);			
	
    //2) get light intensity:
    //Smoothstep is a function that takes a min and a max value, and a third value
    //that if it happens to be between min and max, it will smoothly interpolate 
    //between the two, in a Sigmoid like curve. https://en.wikipedia.org/wiki/Sigmoid_function
    //this means:
    // if min= 0, max = 0.5
    // if input = 1 => output 1
    // if input = -1 => output 0
    // if input = 0.4 => output 0.8-ish
    // if input = 100 => output 1

    //shadowEdgeBlue is often a very small number, and this is used to give 
    //a very sharp shadow edge, but with a smooth transition
    //everthing in the light is 1, and everything in the dark is 0.
    //but the small line inbetween gets interpolated
	float lightIntensity
             = smoothstep(0,shadowEdgeBlur, lightDotNormal * shadow
             /*Unity shadow system =>0 if in shadow, 1 if not*/);	


    //Apply color.
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
    
    //diffuse
	float NdotL = dot(_WorldSpaceLightPos0, normalize(worldSpaceNormal));		
	float lightIntensity = smoothstep(0,0.01, NdotL * shadow);	

    //dot from view direction and light direction
	float3 halfVector = normalize(_WorldSpaceLightPos0 + normalize(viewDirection));

    //dot from normaldirection and the halfvector
	float NdotH = dot(normalize(worldSpaceNormal), halfVector);

    //defines area that should be lit
	float specularIntensity = pow
	(NdotH * lightIntensity, glossiness);

    //Make sharp, toonish line, as explained in the lambertian diffuse 
	float specularIntensitySmooth =
     smoothstep(0.005, 0.01+(specularBlur*0.01), specularIntensity);

    //apply color
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


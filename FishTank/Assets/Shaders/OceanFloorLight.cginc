uniform sampler2D _OceanFloorTexture;
uniform float3 _TextureScroll;


#define TEX_SCALE 0.05

//Half Lambert / "Diffuse Wrapping":

//This function uses a modified "Half-Lambertian" approach to 
//identifying what part of the model should have the "ocean light texture" applied,
//and how much.  
//The light model basically apply's soft ambient light to the dark side.

//https://developer.valvesoftware.com/wiki/Half_Lambert 
//
//"Half Lambert" lighting is a technique first developed in the 
//original Half-Life. It is designed to prevent the rear of an object losing
// its shape and looking too flat. Half Lambert is a completely non-physical
// technique and gives a purely percieved visual enhancement and is an example
// of a forgiving lighting model.
float4 GetOceanLight(fixed worldNormal, float3 worldPos, float shadow, float transparency = 1)
{
    //Get a sample from the ocean floor texture, based
    //on the fragments world position (X & Z)
    //TODO:  scale based on Y
    float4 oceanFloorTex = tex2D(_OceanFloorTexture,
            (worldPos.xz*TEX_SCALE)+_TextureScroll.xy * _Time.yy);

    //from 0-1, how much is the normal facing the light direction?
    float NormalDotLightDifference = max(0.0,dot(normalize(worldNormal),_WorldSpaceLightPos0));
    
    //Modified Half Lambert:
    //Normally a half lambert is ((NdotL * 0.5)+0.5)^2
    //In order to half the lambertian diffuse, and then "add a little back",
    //so that the dark side is never fully dark.
    //i use "transparency" to better control the opacity of the texture
    float Half_Lambert =
     pow(NormalDotLightDifference * transparency + transparency,2);

    //Apply ocean floor
     Half_Lambert*=oceanFloorTex;
    
    //Combine colors multiplicatively
    float4 finalColor = 
    Half_Lambert *
     shadow * //Whether or not the fragment is in the shadow (0=true)
      _LightColor0; //Color of the main directional light

    return finalColor;
}






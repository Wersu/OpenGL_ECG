#version 330

out vec4 outColor;

in vec2 inFragTexCoord;
in vec3 inFragNormal;
in vec3 inWorldPos;

struct BaseLight
{
    vec3 Color;
    float AmbientIntensity;
    float DiffuseIntensity;
};
struct DirectionalLight
{
    BaseLight Base;
    vec3 Direction;
};
struct Attenuation
{
    float Constant;
    float Linear;
    float Exp;
}; 
struct PointLight
{
    BaseLight Base;
    vec3 Position;
    Attenuation Atten;
};
struct SpotLight                                                                           
{                                                                                           
    PointLight Base;                                                                 
    vec3 Direction;                                                                  
    float Cutoff;                                                       
};  

uniform DirectionalLight gDirectionalLight;
uniform PointLight gPointLight;
uniform SpotLight gSpotLight;
uniform sampler2D gSampler;
uniform vec3 gEyeWorldPos;
uniform float gMatSpecularIntensity;
uniform float gSpecularPower; 

vec4 CalcLightInternal(BaseLight Light, vec3 LightDirection, vec3 Normal)             
{                                                                                            
    vec4 AmbientColor = vec4(Light.Color, 1.0f) * Light.AmbientIntensity;                    
    float DiffuseFactor = dot(Normal, -LightDirection);                                      
                                                                                             
    vec4 DiffuseColor  = vec4(0, 0, 0, 0);                                                   
    vec4 SpecularColor = vec4(0, 0, 0, 0);                                                   
                                                                                             
    if (DiffuseFactor > 0) {                                                                 
        DiffuseColor = vec4(Light.Color, 1.0f) * Light.DiffuseIntensity * DiffuseFactor;     
                                                                                             
        vec3 VertexToEye = normalize(gEyeWorldPos - inWorldPos);                              
        vec3 LightReflect = normalize(reflect(LightDirection, Normal));                      
        float SpecularFactor = dot(VertexToEye, LightReflect);                               
        SpecularFactor = pow(SpecularFactor, gSpecularPower);                                
        if (SpecularFactor > 0) {                                                            
            SpecularColor = vec4(Light.Color, 1.0f) *                                        
                            gMatSpecularIntensity * SpecularFactor;                          
        }                                                                                   
    }                                                                                        
                                                                                             
    return (AmbientColor + DiffuseColor + SpecularColor);                                    
}                                                                                          
                                                                                             
vec4 CalcDirectionalLight(vec3 Normal)                                                       
{                                                                                            
    return CalcLightInternal(gDirectionalLight.Base, gDirectionalLight.Direction, Normal);  
}                                                                                            
                                                                                             
vec4 CalcPointLight(PointLight light, vec3 Normal)                                                  
{                                                                                            
    vec3 LightDirection = light.Position - inWorldPos;                          
    float Distance = length(LightDirection);                                                 
    LightDirection = normalize(LightDirection);                                              
                                                                                             
    vec4 Color = CalcLightInternal(light.Base, LightDirection, Normal);        
    float Attenuation =  light.Atten.Constant +                                
                         light.Atten.Linear * Distance +                       
                         light.Atten.Exp * Distance * Distance;                
                                                                                             
    return Color / Attenuation;                                                              
}                      

vec4 CalcSpotLight(SpotLight l, vec3 Normal)                                          
{                                                                                            
    vec3 LightToPixel = normalize(inWorldPos - l.Base.Position);                              
    float SpotFactor = dot(LightToPixel, l.Direction);                                       
                                                                                             
    if (SpotFactor > l.Cutoff) {                                                             
        vec4 Color = CalcPointLight(l.Base, Normal);                                         
        return Color * (1.0 - (1.0 - SpotFactor) * 1.0/(1.0 - l.Cutoff));                    
    }                                                                                        
    else {                                                                                   
        return vec4(0,0,0,0);                                                                
    }                                                                                        
}    
                                                                                             
void main()                                                                                  
{                                                                                            
    vec3 Normal = normalize(inFragNormal);                                                        
    vec4 TotalLight = CalcDirectionalLight(inFragNormal);      
    TotalLight += CalcPointLight(gPointLight,inFragNormal);
    TotalLight += CalcSpotLight(gSpotLight,inFragNormal);
                                                                                             
    outColor = texture2D(gSampler, inFragTexCoord.xy) * TotalLight;                              
}
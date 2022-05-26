#version 330 

layout (location=0) in vec3 aPosition;
layout (location=1) in vec2 TexCoord;
layout (location=2) in vec3 Normal;

uniform mat4 transform;
uniform mat4 gWorld;

out vec2 inFragTexCoord;
out vec3 inFragNormal;
out vec3 inWorldPos;

void main()
{
    gl_Position =  transform * vec4(aPosition, 1.0);
    inFragTexCoord = TexCoord;
    inFragNormal   = (gWorld * vec4(Normal, 0.0)).xyz;
    inWorldPos = (gWorld * vec4(aPosition, 1.0)).xyz;
}
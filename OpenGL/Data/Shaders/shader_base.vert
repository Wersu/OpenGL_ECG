#version 330

layout (location=0) in vec3 aPosition;

uniform mat4 transform;

out vec4 inFragColor;

void main()
{
    inFragColor = vec4(aPosition, 1);    
    gl_Position =  transform * vec4(aPosition, 1.0);
}
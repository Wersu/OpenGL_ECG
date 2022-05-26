#version 330

out vec4 outColor;

in vec4 aColor;

in vec4 inFragColor;

void main()
{
    outColor = inFragColor;
}
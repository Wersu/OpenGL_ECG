using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OpenGL.Shadow
{
    public class ShadowTechnique
    {
        private int wvpIndex;
        private int textureIndex;

        public void Initialize(Shader.ShaderProgram shader)
        {
            wvpIndex = GL.GetUniformLocation(shader.Program, "gWVP");
            textureIndex = GL.GetUniformLocation(shader.Program, "gShadowMap");
        }

        public void SetWVP()
        {
            Matrix4 wvp = Pipeline.Pipeline.GetWorldTransformation();
            GL.UniformMatrix4(wvpIndex, true, ref wvp);
        }

        public void SetTextureUnit(int texture)
        {
            GL.Uniform1(textureIndex, texture);
        }
    }
}

using OpenGL.Shader;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL.Lightning.Variables
{
    public class DirectionalLight : Lightning
    {
        public BaseLight light = new BaseLight() { 
          Color = new Vector3(1,1,1),
          DiffuseIntensity = 0.05f,
          AmbientIntensity = 0f,
        };

        protected Vector3 _rotation = new Vector3(-30,-30,-30);//направление

        private int colorIndex;
        private int ambientIntensityIndex;
        private int rotationIndex;
        private int diffuseIntensityIndex;

        public override void Initialize(ShaderProgram shader)
        {
            base.Initialize(shader);

            colorIndex = GL.GetUniformLocation(shader.Program, "gDirectionalLight.Base.Color");
            ambientIntensityIndex = GL.GetUniformLocation(shader.Program, "gDirectionalLight.Base.AmbientIntensity");
            rotationIndex = GL.GetUniformLocation(shader.Program, "gDirectionalLight.Direction");
            diffuseIntensityIndex = GL.GetUniformLocation(shader.Program, "gDirectionalLight.Base.DiffuseIntensity");
        }

        public override void Update()
        {
            base.Update();

            _lightShader.SetVector3(colorIndex, light.Color);
            _lightShader.SetFloat(ambientIntensityIndex, light.AmbientIntensity);
            _lightShader.SetVector3(rotationIndex, _rotation);
            _lightShader.SetFloat(diffuseIntensityIndex, light.DiffuseIntensity);
        }
    }
}

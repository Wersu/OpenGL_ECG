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
    public class PointLight : Lightning
    {
        public BaseLight light = new BaseLight()
        {
            Color = new Vector3(1, 0, 0),
            DiffuseIntensity = 0.5f,
            AmbientIntensity = 50f,
        };

        public Vector3 Position = new Vector3(1,0,1);

        protected Attenuation atten = new Attenuation() { Constant = 0,Linear = 0f, Exp=100f };

        private int colorIndex;  
        private int ambientIndex;
        private int diffuceIndex;

        private int constantIndex;
        private int linearIndex;
        private int expIndex;

        private int positionIndex;

        public override void Initialize(ShaderProgram shader)
        {
            base.Initialize(shader);

            positionIndex = GL.GetUniformLocation(shader.Program, "gPointLight.Position");
            colorIndex = GL.GetUniformLocation(shader.Program, "gPointLight.Base.Color");
            ambientIndex = GL.GetUniformLocation(shader.Program, "gPointLight.Base.AmbientIntensity");
            diffuceIndex = GL.GetUniformLocation(shader.Program, "gPointLight.Base.DiffuseIntensity");
            constantIndex = GL.GetUniformLocation(shader.Program, "gPointLight.Atten.Constant");
            linearIndex = GL.GetUniformLocation(shader.Program, "gPointLight.Atten.Linear");
            expIndex = GL.GetUniformLocation(shader.Program, "gPointLight.Atten.Exp");
        }

        public override void Update()
        {
            base.Update();

            _lightShader.SetVector3(positionIndex,Position);
            _lightShader.SetVector3(colorIndex,light.Color);
            _lightShader.SetFloat(ambientIndex,light.AmbientIntensity);
            _lightShader.SetFloat(diffuceIndex,light.DiffuseIntensity);
            _lightShader.SetFloat(constantIndex,atten.Constant);
            _lightShader.SetFloat(linearIndex,atten.Linear);
            _lightShader.SetFloat(expIndex,atten.Exp);

        }
    }
}

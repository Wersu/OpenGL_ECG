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
    public class SpotLight : Lightning
    {
        protected Vector3 direction = new Vector3(0, 90, 0);
        protected BaseLight light = new BaseLight()
        {
            Color = new Vector3(1,0,1),
            DiffuseIntensity = 100f,
            AmbientIntensity = 3f,
        };
        protected Vector3 position = new Vector3(0.0f, -1f, 0f);
        protected float cutoff = 80.0f; 
        protected Attenuation atten = new Attenuation() { Constant=0, Linear =0,Exp=1f };

        private int colorIndex;
        private int ambientIndex;
        private int diffuseIndex;
        private int posIndex;
        private int rotationIndex;
        private int cutoffIndex;
        private int constantIndex;
        private int linearIndex;
        private int expIndex;

        public override void Initialize(ShaderProgram shader)
        {
            base.Initialize(shader);

            colorIndex = GL.GetUniformLocation(shader.Program, "gSpotLight.Base.Base.Color");
            ambientIndex = GL.GetUniformLocation(shader.Program, "gSpotLight.Base.Base.AmbientIntensity");
            diffuseIndex = GL.GetUniformLocation(shader.Program, "gSpotLight.Base.Base.DiffuseIntensity");
            constantIndex = GL.GetUniformLocation(shader.Program, "gSpotLight.Base.Atten.Constant");
            linearIndex = GL.GetUniformLocation(shader.Program, "gSpotLight.Base.Atten.Linear");
            expIndex = GL.GetUniformLocation(shader.Program, "gSpotLight.Base.Atten.Exp");
            posIndex = GL.GetUniformLocation(shader.Program, "gSpotLight.Base.Position");
            rotationIndex = GL.GetUniformLocation(shader.Program, "gSpotLight.Direction");
            cutoffIndex = GL.GetUniformLocation(shader.Program, "gSpotLight.Cutoff");
        }

        public override void Update()
        {
            base.Update();
            
            _lightShader.SetVector3(posIndex, position);
            _lightShader.SetVector3(colorIndex, light.Color);
            _lightShader.SetFloat(ambientIndex,light.AmbientIntensity);
            _lightShader.SetFloat(diffuseIndex,light.DiffuseIntensity);
            _lightShader.SetFloat(constantIndex,atten.Constant);
            _lightShader.SetFloat(linearIndex,atten.Linear);
            _lightShader.SetFloat(expIndex,atten.Exp);
            _lightShader.SetFloat(cutoffIndex, cutoff);
            _lightShader.SetVector3(rotationIndex, direction);
        }
    }
}

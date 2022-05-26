using OpenGL.Shader;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL.Lightning
{
    public abstract class Lightning
    {
        private static ShaderProgram lightShader;
        public static float specularIntensity { get; set; } = 1f;
        public static float specularPower { get; set; } = 32f;

        private static int eyeWorldPosIndex;
        private static int matSpecularIntensityIndex;
        private static int matSpecularPowerIndex;

        public static void InitializeGlobal(ShaderProgram shader)
        {
            lightShader = shader;

            eyeWorldPosIndex = GL.GetUniformLocation(lightShader.Program, "gEyeWorldPos");
            matSpecularIntensityIndex = GL.GetUniformLocation(lightShader.Program, "gMatSpecularIntensity");
            matSpecularPowerIndex = GL.GetUniformLocation(lightShader.Program, "gSpecularPower");
        }

        public static void UpdateGlobal()
        {
            lightShader.SetMatrix4("gWorld", Pipeline.Pipeline.GetWorldTransformation());

            lightShader.SetFloat(matSpecularIntensityIndex, specularIntensity);
            lightShader.SetFloat(matSpecularPowerIndex, specularPower);
            lightShader.SetVector3(eyeWorldPosIndex, Camera.Camera.Position);
        }


        protected ShaderProgram _lightShader;

        public virtual void Initialize(ShaderProgram shader) 
        {
            _lightShader = shader;
            lightShader = shader;
        }
         
        public virtual void Update() 
        {
        }

        public struct Attenuation
        {
            public float Constant;
            public float Linear;
            public float Exp;
            public Attenuation()
            {
                Constant = 1.0f;
                Linear = 0.1f;
                Exp = 0.0f;
            }
        }

        public struct BaseLight
        {
            public Vector3 Color;
            public float AmbientIntensity;
            public float DiffuseIntensity;
        };
    }
}

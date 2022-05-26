using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL.Shader
{
    public class ShaderProgram
    {
        private int _program = 0;
        public ShaderProgram(List<ShaderPath> paths)
        {
            Initialize(paths);
        }

        private void Initialize(List<ShaderPath> paths)
        {
            List<int> shadersID = new List<int>();

            foreach (var item in paths)
                shadersID.Add(CreateShader(item));

            _program = GL.CreateProgram();

            foreach (var item in shadersID)
                GL.AttachShader(_program, item);

            GL.LinkProgram(_program);
            GL.GetProgram(_program, GetProgramParameterName.LinkStatus, out int statusCode);
            if (statusCode != (int)All.True)
            {
                string infoLog = GL.GetProgramInfoLog(_program);
                throw new Exception($"Program [{_program}] link error \n\n{infoLog}");
            }

            foreach(var item in shadersID)
                DeleteShader(item);
        }

        public void Enable() => GL.UseProgram(_program);

        public void Disable() => GL.UseProgram(0);

        public void SetMatrix4(string name, Matrix4 value)
        {
            int location = GL.GetUniformLocation(_program, name);
            GL.UniformMatrix4(location,true,ref value);
        }

        private void DeleteProgram() => DeleteProgram(_program);

        private void DeleteShader(int shaderID) => DeleteShader(_program, shaderID);

        private static void DeleteProgram(int id) => GL.DeleteProgram(id);

        private static int CreateShader(ShaderPath path)
        {
            string shaderCode = File.ReadAllText(path.Path.ToString());
            int shaderID = GL.CreateShader(path.Type);

            GL.ShaderSource(shaderID, shaderCode);
            GL.CompileShader(shaderID);

            GL.GetShader(shaderID, ShaderParameter.CompileStatus, out int statusCode);
            if (statusCode != (int)All.True)
            {
                string infoLog = GL.GetShaderInfoLog(shaderID);
                throw new Exception($"Shader [{shaderID}] compile error \n\n{infoLog}");
            }

            return shaderID;
        }

        private static void DeleteShader(int programID,int shaderID)
        {
            GL.DetachShader(programID,shaderID);
            GL.DeleteShader(shaderID);
        }

        ~ShaderProgram()
        {
            DeleteProgram();
        }
    }
}

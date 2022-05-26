using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL.Shader
{
    public class ShaderPath
    {
        public readonly string Path;
        public readonly ShaderType Type;
        public ShaderPath(string path,ShaderType type)
        {
            Path = path;
            Type = type;
        }
    }
}

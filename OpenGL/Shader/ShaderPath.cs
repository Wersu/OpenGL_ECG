using OpenTK.Graphics.OpenGL;

namespace OpenGL.Shader
{
    public class ShaderPath
    {
        public readonly string Path;
        public readonly ShaderType Type;
        public ShaderPath(string path, ShaderType type)
        {
            Path = path;
            Type = type;
        }
    }
}

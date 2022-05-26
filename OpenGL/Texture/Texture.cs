using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using ImageMagick;
using OpenTK.Graphics.OpenGL;

namespace OpenGL.Texture
{
    public class Texture
    {
        private readonly string path;

        private MagickImage img;

        private int _textureIndex;

        public Texture(string Path)
        {
            path = Path;
        }

        public void Initialize()
        {
            img = new MagickImage(path);
            img.Write(path, MagickFormat.Rgba);

            byte[] pixels = img.GetPixels().ToArray();

            GL.GenTextures(1, out _textureIndex);
            GL.BindTexture(TextureTarget.Texture2D, _textureIndex);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, img.Width, img.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, pixels);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        }

        public void Enable(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, _textureIndex);
        }
    } 
}

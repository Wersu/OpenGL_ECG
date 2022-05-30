using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL.Shadow
{
    public class ShadowFBO : GameComponent
    {
        protected int fboIndex;
        private int mapIndex;
        public override void Initialize(Game game)
        {
            base.Initialize(game);

            GL.GenTextures(1, out fboIndex);

            GL.GenTextures(1, out mapIndex);
            GL.BindTexture(TextureTarget.Texture2D, mapIndex);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent, _game.Size.X, _game.Size.Y, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear/*(int)TextureMagFilter.Linear*/);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);

            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, fboIndex);
            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, mapIndex, 0);
            GL.DrawBuffer(0);
            GL.ReadBuffer(0);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void BindForWriting()
        {
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, fboIndex);
        }

        public void BindForReading(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, mapIndex);
        }
    }
}

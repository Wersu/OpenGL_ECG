using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL.KeyListener.Variables
{
    internal class GLRenderSwaper : IKeyListener
    {
        public void KeyPressed(KeyboardState state)
        {
            if (state.IsKeyPressed(Keys.D1))
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            if (state.IsKeyPressed(Keys.D2))
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            if (state.IsKeyPressed(Keys.D3))
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Point);
            if (state.IsKeyPressed(Keys.D4))
                GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
            if (state.IsKeyPressed(Keys.D5))
                GL.PolygonMode(MaterialFace.Front, PolygonMode.Line);
            if (state.IsKeyPressed(Keys.D6))
                GL.PolygonMode(MaterialFace.Front, PolygonMode.Point);
            if (state.IsKeyPressed(Keys.D7))
                GL.PolygonMode(MaterialFace.Back, PolygonMode.Fill);
            if (state.IsKeyPressed(Keys.D8))
                GL.PolygonMode(MaterialFace.Back, PolygonMode.Line);
            if (state.IsKeyPressed(Keys.D9))
                GL.PolygonMode(MaterialFace.Back, PolygonMode.Point);
        }
    }
}

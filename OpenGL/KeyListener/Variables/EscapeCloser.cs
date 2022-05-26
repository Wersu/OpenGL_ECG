using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL.KeyListener.Variables
{
    internal class EscapeCloser : GameComponent, IKeyListener
    {
        public void KeyPressed(KeyboardState state)
        {
            if (!state.IsKeyDown(Keys.Escape))
                return;

            _game.Close();
        }
    }
}

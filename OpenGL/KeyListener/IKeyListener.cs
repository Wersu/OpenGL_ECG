using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL.KeyListener
{
    internal interface IKeyListener
    {
        void KeyPressed(KeyboardState state);
    }
}

using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL.InputListener
{
    internal interface IMouseListener
    {
        void OnMouseClick(MouseState state);

        void OnMouseMove(MouseState state);
    }
}

using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenGL.InputListener
{
    internal interface IKeyListener
    {
        void KeyPressed(KeyboardState state);
    }
}

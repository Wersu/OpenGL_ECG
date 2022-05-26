using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenGL.InputListener.Variables
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

using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL.KeyListener.Variables
{
    internal class GLRotator : GameComponent, IKeyListener
    {
        private const float ROTATION_SPEED = 0.5f;

        public void KeyPressed(KeyboardState state)
        {
            if (!state.IsKeyDown(Keys.Left)&&!state.IsKeyDown(Keys.Right)&&!state.IsKeyDown(Keys.Down)&&!state.IsKeyDown(Keys.Up))
                return;

            Vector3 rotation = Vector3.Zero; 

            if (state.IsKeyDown(Keys.Up))
                rotation.X=1;
            if (state.IsKeyDown(Keys.Down))
                rotation.X=-1;
            if (state.IsKeyDown(Keys.Right))
                rotation.Y=-1;
            if (state.IsKeyDown(Keys.Left))
                rotation.Y=1;

            rotation.Normalize();

            _game.RendererElements.ForEach(x => x.Rotation += rotation*ROTATION_SPEED);
        }
    }
}

using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL.InputListener.Variables
{
    internal class CameraController : GameComponent , IKeyListener , IMouseListener
    {
        private const float MOVE_SPEED = 0.025f;//скорость движения камеры
        private const float ROTATION_SPEED = 0.001f;//скорость поворота камеры

        public void KeyPressed(KeyboardState state)
        {
            Vector3 position = Vector3.Zero;//дельта позиции

            float moveSpeed = MOVE_SPEED;

            if (state.IsKeyDown(Keys.LeftShift))
                moveSpeed *= 4;//если нажат Shift, то увеличиваем скорость в 4 раза

            if (state.IsKeyDown(Keys.W))
                position -= Camera.Camera.Forward*moveSpeed;//если нажата W, то движемся вперед 
            if(state.IsKeyDown(Keys.S))
                position += Camera.Camera.Forward*moveSpeed;//если нажата S, то движемся назад
            if (state.IsKeyDown(Keys.A))
                position += Camera.Camera.Left*moveSpeed;//если нажата A, то движемся влево
            if (state.IsKeyDown(Keys.D))
                position += Camera.Camera.Right*moveSpeed;//если нажата D, то движемся вправо

            if (state.IsKeyDown(Keys.Space))
                position += Camera.Camera.Up*moveSpeed;//если нажат пробел, то движемся вверх
            if(state.IsKeyDown(Keys.LeftControl))
                position -= Camera.Camera.Up*moveSpeed;//если нажат Control, то движемся вниз

            Camera.Camera.Position += position;//прибавляем дельту к позиции камеры
        }

        public void OnMouseClick(MouseState state)
        {
           
        }

        public void OnMouseMove(MouseState state)
        {
            Vector3 Axis = Vector3.Zero;

            if (state.Delta!=Vector2.Zero)
            {
                Axis.X -= state.Delta.X*ROTATION_SPEED;
                Axis.Y += state.Delta.Y*ROTATION_SPEED;
            }
            else
            {
                if (state.Position.X<=5)
                    Axis.X += ROTATION_SPEED*10f;
                if (state.Position.X >= _game.Size.X-5)
                    Axis.X -= ROTATION_SPEED*10f;
                if(state.Position.Y<=5)
                    Axis.Y -= ROTATION_SPEED*10f;
                if (state.Position.Y>=_game.Size.Y-5)
                    Axis.Y += ROTATION_SPEED*10f;
            }

            if(Axis!=Vector3.Zero)
                Camera.Camera.Rotation += Axis;
        }
    }
}

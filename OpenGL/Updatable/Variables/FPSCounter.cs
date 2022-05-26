using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL.Updatable.Variables
{
    internal class FPSCounter : GameComponent , IFrameUpdatable
    {
        private const float UPDATE_RATE = 1f;

        private float frameTime;
        private int fps = 0;

        public event Action<int> OnFPSUpdated;

        public void Update(FrameEventArgs args)
        {
            frameTime += (float)args.Time;//прошедшее время
            fps++;//количество кадров

            if (frameTime>+UPDATE_RATE)//если прошло UPDATE_RATE секунд
            {
                OnFPSUpdate();

                fps = 0;
                frameTime = 0;
            }
        }

        private void OnFPSUpdate()
        {
            _game.Title = $"OpenGL [{fps} FPS]";

            OnFPSUpdated?.Invoke(fps);
        }
    }
}

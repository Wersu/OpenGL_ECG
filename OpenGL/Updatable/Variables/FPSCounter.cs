using OpenTK.Windowing.Common;

namespace OpenGL.Updatable.Variables
{
    internal class FPSCounter : GameComponent, IFrameUpdatable
    {
        private const float UpDATE_RATE = 1f;

        private float frameTime;
        private int fps = 0;

        public event Action<int> OnFPSUpdated;

        public void Update(FrameEventArgs args)
        {
            frameTime += (float)args.Time;//прошедшее время
            fps++;//количество кадров

            if (frameTime>+UpDATE_RATE)//если прошло UpDATE_RATE секунд
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

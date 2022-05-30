using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace OpenGL.Camera
{
    public static class Camera
    {
        public static float FOV = 60f;//угол обзора (field of view)
        public static float Z_NEAR = 0.1f;//Позиция ближней Z плоскости. Позволяет нам обрезать объекты, находящиеся слишком близко к камере.
        public static float Z_FAR = 1000;//Позиция дальней Z плоскости. Позволяет нам обрезать объекты, находящиеся слишком далеко от камеры.
        public static Vector3 Up = new Vector3(0, 1, 0);//верх камеры

        private static float _width, _height;//ширина-высота игрового окна

        private static Vector3 _position = new Vector3(0, 0, 2);//позиция камеры
        private static Vector3 _rotation = new Vector3(0, 0, 1);//поворот камеры

        public static float Width { get { return _width; } }
        public static float Height { get { return _height; } }

        public static Vector3 Position { get { return _position; } set { _position=value; OnCameraChanged?.Invoke(); } }
        public static Vector3 Rotation { get { return _rotation; } set { _rotation=value; OnCameraChanged?.Invoke(); } }

        public static event Action OnCameraChanged;

        public static Vector3 Forward => Rotation.Normalized();
        public static Vector3 Left => Vector3.Cross(Forward, Up).Normalized();
        public static Vector3 Right => Vector3.Cross(Up, Forward).Normalized();

        public static void Initialize(GameWindow game)
        {
            _width = game.Size.X;
            _height = game.Size.Y;
        }
    }
}

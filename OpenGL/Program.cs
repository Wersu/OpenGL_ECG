using System;

using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenGL.Updatable;
using OpenGL.Updatable.Variables;
using OpenGL.KeyListener;
using OpenGL.KeyListener.Variables;
using OpenGL.Renderer;
using OpenGL.Renderer.Variables;
using OpenGL.Shader;

namespace OpenGL
{
    public class Game : GameWindow
    {
        List<IFrameUpdatable> updatableElements = new List<IFrameUpdatable>();
        List<IKeyListener> keyListenerElements = new List<IKeyListener>() {
            new GLRenderSwaper(),//меняем режимы
        };
        List<RendererComponent> rendererElements = new List<RendererComponent>() {
            new PyramidRenderer(),
        };

        public List<RendererComponent> RendererElements { get { return rendererElements; } }

        private ShaderProgram shader;
        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
                : base(gameWindowSettings, nativeWindowSettings)
        {
            Console.WriteLine(GL.GetString(StringName.Version));
            Console.WriteLine(GL.GetString(StringName.Vendor));
            Console.WriteLine(GL.GetString(StringName.Renderer));
            Console.WriteLine(GL.GetString(StringName.ShadingLanguageVersion));

            VSync = VSyncMode.On;//вертикальная синхронизация (ограничение fps)
        }

        protected override void OnLoad()
        {
            GL.ClearColor(.1f, .7f, .1f, 1);//Указываем стандартный цвет, который будет использоваться после очистки

            FPSCounter fpsCounter = new FPSCounter();
            fpsCounter.Initialize(this);
            updatableElements.Add(fpsCounter);

            EscapeCloser escapeCloser = new EscapeCloser();//добавляем закрытие игрового окна при нажатии ESC
            escapeCloser.Initialize(this);
            keyListenerElements.Add(escapeCloser);

            GLRotator rotator = new GLRotator();
            rotator.Initialize(this);
            keyListenerElements.Add(rotator);

            shader = new ShaderProgram(new List<ShaderPath>()
            {
                new ShaderPath("Data/Shaders/shader_base.vert",ShaderType.VertexShader),
                new ShaderPath("Data/Shaders/shader_base.frag",ShaderType.FragmentShader),
            });

            rendererElements.ForEach(x => x.Initialize(shader));

            GL.Enable(EnableCap.CullFace);//разблокируем функцию GL.CullFace()
            GL.CullFace(CullFaceMode.Back);//включаем отбрасывание задних граней

            base.OnLoad();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            updatableElements.ForEach(x => x.Update(args));

            if (KeyboardState.IsAnyKeyDown)
                keyListenerElements.ForEach(x => x.KeyPressed(KeyboardState));

            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);//Очищает буффер цвета, Очищает кажный кадр нашу форму

            shader.Enable();

            rendererElements.ForEach(x => x.Update());

            shader.Disable();

            SwapBuffers();//Реализует двойную буферизацию, смена буферов

            base.OnRenderFrame(args);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var nativeSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(800, 800),
                Location = new Vector2i(100, 100),
                WindowBorder = WindowBorder.Fixed,
                WindowState = WindowState.Normal,
                Title = "OpenGL",

                Flags = ContextFlags.Default,
                APIVersion = new Version(3, 3),
                Profile = ContextProfile.Compatability,
                API = ContextAPI.OpenGL,

                IsFullscreen = false,
                NumberOfSamples = 0
            };

            using (Game game = new Game(GameWindowSettings.Default, nativeSettings))
            {
                game.Run();
            }
        }
    }
}
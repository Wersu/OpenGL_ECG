using OpenGL.InputListener;
using OpenGL.InputListener.Variables;
using OpenGL.Renderer;
using OpenGL.Renderer.Variables;
using OpenGL.Shader;
using OpenGL.Updatable;
using OpenGL.Updatable.Variables;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenGL
{
    public class Game : GameWindow
    {
        List<IFrameUpdatable> UpdatableElements = new List<IFrameUpdatable>();//создаем список обновляемых элементов
        List<IKeyListener> keyListenerElements = new List<IKeyListener>() //создаем список прослушивемых элементов с клавиатуры
        {
            new GLRenderSwaper(),//меняем режимы    
        };
        List<IMouseListener> mouseListenerElements = new List<IMouseListener>();//создаем список прослушивемых действий мыши

        List<RendererComponent> rendererElements = new List<RendererComponent>() //создаем список элементов, которые будут рендериться
        {
            new CubePolygonRenderer(new Vector3(0,0,0),new Vector3(1,1,1)),//создаем куб(1. позиция, где будет находиться объект, 2. масштаб  объекта)
            new CubePolygonRenderer(new Vector3(0,2,0),new Vector3(10,1,10)),//создаем куб(1. позиция, где будет находиться объект, 2. масштаб  объекта)                     
        };
         
        List<Texture.Texture> textures = new List<Texture.Texture>() //создаем список текстур
        {
            new Texture.Texture("Data/Textures/texture.png"),
        };

        List<Lightning.Lightning> lights = new List<Lightning.Lightning>() 
        { 
            new Lightning.Variables.DirectionalLight(), 
            new Lightning.Variables.Reflect(), 
            new Lightning.Variables.PointLight(),
            new Lightning.Variables.SpotLight(),
        };

        public List<Lightning.Lightning> Lights { get { return lights; } }

        public List<RendererComponent> Renderers { get { return rendererElements; } }

        private ShaderProgram shader;//создаем шейдерную программу
        private ShaderProgram shadowShader;

        private Shadow.ShadowFBO shadowFBO = new Shadow.ShadowFBO();
        private Shadow.ShadowTechnique shadowTech = new Shadow.ShadowTechnique();

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
            GL.ClearColor(.75f, .75f, .75f, 1);//Указываем стандартный цвет, который будет использоваться после очистки

            GL.Enable(EnableCap.CullFace);//разблокируем функцию GL.CullFace()
            GL.Enable(EnableCap.Texture2D);

            GL.Enable(EnableCap.DepthTest);

            GL.CullFace(CullFaceMode.Back);//включаем отбрасывание задних граней

            shadowFBO.Initialize(this);

            Camera.Camera.Initialize(this);//инициализируем камеру   
            Camera.Camera.OnCameraChanged += OnCameraChanged;//подписываем наш метод на событие изменения камеры

            FPSCounter fpsCounter = new FPSCounter();//создаем счетчик fps
            fpsCounter.Initialize(this);//инициализируем счетчик fps
            UpdatableElements.Add(fpsCounter);//добаляем в список обновляемых элементов счетчик fps 

            EscapeCloser escapeCloser = new EscapeCloser();//добавляем закрытие игрового окна при нажатии ESC
            escapeCloser.Initialize(this);
            keyListenerElements.Add(escapeCloser);//добаляем в список прослушиваемых элементов клавиатуры 

            CameraController cameraController = new CameraController();//создаем управляемую камеру
            cameraController.Initialize(this);
            keyListenerElements.Add(cameraController);//добаляем в список прослушиваемых элементов клавиатуры 
            mouseListenerElements.Add(cameraController);//добаляем в список прослушиваемых элементов мыши

            DirectionalLightController lightController = new DirectionalLightController();
            lightController.Initialize(this);
            keyListenerElements.Add(lightController);

            shader = new ShaderProgram(new List<ShaderPath>()//создаем шейдерные программы
            {
                new ShaderPath("Data/Shaders/shader_base.vert",ShaderType.VertexShader),//создаем шейдерную программу по указанному пути и задаем тип шейдера. Вершинный шейдер получает
                                                                                        //одну вершину из потока вершин и генерирует одну вершину в выходной поток вершин
                new ShaderPath("Data/Shaders/shader_base.frag",ShaderType.FragmentShader),//Фрагментный шейдер обрабатывает фрагмент, сгенерированный растеризацией, в набор цветов и одно значение глубины.
            });

            shadowShader = new ShaderProgram(new List<ShaderPath>() {
                new ShaderPath("Data/Shaders/shader_shadow.vert",ShaderType.VertexShader),//создаем шейдерную программу по указанному пути и задаем тип шейдера. Вершинный шейдер получает
                                                                                        //одну вершину из потока вершин и генерирует одну вершину в выходной поток вершин
                new ShaderPath("Data/Shaders/shader_shadow.frag",ShaderType.FragmentShader),//Фрагментный шейдер обрабатывает фрагмент, сгенерированный растеризацией, в набор цветов и одно значение глубины.
            });

            shadowTech.Initialize(shadowShader);
            shadowShader.Enable();

            textures.ForEach(x => x.Initialize());//инициализизуем текстуры

            Lightning.Lightning.InitializeGlobal(shader);

            lights.ForEach(x => x.Initialize(shader));

            rendererElements.ForEach(x => x.Initialize(shader, textures[0]));//инициализируем элементы, которые должны отрендериться

            base.OnLoad();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)//обновляем окно
        {
            UpdatableElements.ForEach(x => x.Update(args));//обновляем элементы списка

            if (KeyboardState.IsAnyKeyDown)
                keyListenerElements.ForEach(x => x.KeyPressed(KeyboardState));//если нажата какая-либо кнопка, то проверяем прослушивается ли она и делаем соответствующие действия

            if (MouseState.IsAnyButtonDown)
                mouseListenerElements.ForEach(x => x.OnMouseClick(MouseState));//проверяем нажата ли кнопка мыши и делаем соответствующие действия

            if (WindowState == WindowState.Maximized||(MouseState.Position.X < Size.X && MouseState.Position.X > 0 && MouseState.Position.Y < Size.Y && MouseState.Position.Y > 0))
                mouseListenerElements.ForEach(x => x.OnMouseMove(MouseState));//если окно развернуто или мыши находится внутри окна, то делаем соответстыующие действия при движении мыши

            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            ShadowMapPass();
            Render();

            base.OnRenderFrame(args);
        }

        private void ShadowMapPass()
        {
            GL.Viewport(0, 0, 1024, 1024);
            shadowFBO.BindForWriting();

            GL.Clear(ClearBufferMask.DepthBufferBit);


            Vector3 startCameraPos = Camera.Camera.Position;
            Vector3 startCameraRotation = Camera.Camera.Rotation;
            shadowTech.SetWVP();
            shadowShader.Enable();

            public static Matrix4 lightProjection = Matrix4.CreateOrthographic(-10.0f, 10.0f, 1f, 7.5f);
            public static Matrix4 lightView = Matrix4.LookAt(new Vector3(-2.0f, 4.0f, -1.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            public Matrix4 lightSpaceMatrix = lightProjection * lightView;


        //shadowFBO.BindForWriting();

        //GL.Clear(ClearBufferMask.DepthBufferBit);

        //shadowShader.Enable();

        //Vector3 startCameraPos = Camera.Camera.Position;
        //Vector3 startCameraRotation = Camera.Camera.Rotation;

        //Lightning.Variables.SpotLight spotLight = lights[3] as Lightning.Variables.SpotLight;

        //Matrix4 lightProgection = Matrix4.CreateOrthographic(10f, -10f, 1f, 7.5f);
        //Matrix4 lightView = Matrix4.LookAt(spotLight.position, Vector3.Zero, new Vector3(0, 1, 0));
        //Matrix4 lightSpaceMatrix = lightProgection * lightView;



        //Pipeline.Pipeline.SetScale(new Vector3(0.2f, 0.2f, 0.2f));
        //Pipeline.Pipeline.SetPosition(new Vector3(0, 0, 5));
        //Camera.Camera.Position = spotLight.position;
        //Camera.Camera.Rotation = spotLight.direction;
        //Camera.Camera.FOV = 60;
        //Camera.Camera.Z_NEAR = 1;
        //Camera.Camera.Z_FAR = 50;
        //shadowTech.SetWVP();

        //rendererElements.ForEach(x =>
        //{
        //    x.Update();
        //});

        //GL.BindFramebuffer(FramebufferTarget.Framebuffer,0);

        //Pipeline.Pipeline.SetScale(new Vector3(1,1,1));
        //Pipeline.Pipeline.SetPosition(new Vector3(0, 0, 0));
        //Camera.Camera.Position = startCameraPos;
        //Camera.Camera.Rotation = startCameraPos;
        //Camera.Camera.FOV = 60;
        //Camera.Camera.Z_NEAR = 0.1f;
        //Camera.Camera.Z_FAR = 1000;
    }

        private void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);//Очищает буфер цвета, Очищает кажный кадр нашу форму


            shadowFBO.BindForReading(TextureUnit.Texture1);

            shadowTech.SetWVP();

            shader.Enable();//открываем доступ к шейдерной программе

            rendererElements.ForEach(x =>
            {
                x.Update();
            });//обновляем список элементов, которые должны отрендериться

            lights.ForEach(x => x.Update());

            Lightning.Lightning.UpdateGlobal();

            shader.Disable();//закрываем доступ к программе

            SwapBuffers();//Реализует двойную буферизацию, смена буферов
        }

        private void OnCameraChanged()
        {
            rendererElements.ForEach(x => x.UpdateTransformation());
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var nativeSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(1920, 1080),
                Location = new Vector2i(0, 0),
                WindowBorder = WindowBorder.Hidden,
                WindowState = WindowState.Maximized,
                Title = "OpenGL",

                Flags = ContextFlags.Default,
                APIVersion = new Version(3, 3),
                Profile = ContextProfile.Compatability,
                API = ContextAPI.OpenGL,

                IsFullscreen = true,
                NumberOfSamples = 0
            };

            using (Game game = new Game(GameWindowSettings.Default, nativeSettings))
            {
                game.Run();
            }
        }
    }
}
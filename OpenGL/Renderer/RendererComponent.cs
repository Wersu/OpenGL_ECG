using OpenGL.Shader;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace OpenGL.Renderer
{
    public abstract class RendererComponent
    {
        protected ShaderProgram _transformShader;//шейдер для отображения
        protected int _vboVerticesIndex;//индификатор, в котором хранятся вершины
        protected int _vboColorsIndex;//индификатор, в котором хранятся цвета
        protected uint _iboIndex;//индификатор, в котором хранятся индексы вершин

        protected Vector3 _position = new Vector3(0,0,2);//позиция объекта
        protected Vector3 _rotation = new Vector3(25,10,5);//поворот объекта
        protected Vector3 _scale = Vector3.One;//масштаб  объекта
        protected Matrix4 _transformation = Matrix4.Identity;//финальное положение объекта

        public abstract float[] Vertices { get; set; }
        public abstract float[] Colors { get; set; }
        public abstract uint[] Indecies { get; set; }

        public Vector3 Position { get { return _position; } set { _position=value; Transformation = Pipeline.Pipeline.GetTransformation(this); } }
        public Vector3 Rotation { get { return _rotation; } set { _rotation=value; Transformation = Pipeline.Pipeline.GetTransformation(this); } }
        public Vector3 Scale { get { return _scale; } set { _scale=value; Transformation = Pipeline.Pipeline.GetTransformation(this); } }
        public Matrix4 Transformation { get { return _transformation; } set { _transformation=value; } }

        protected virtual PrimitiveType RenderType { get; set; } = PrimitiveType.TriangleStrip;

        public virtual int VerticesCount { get { return Indecies.Length; } }

        public virtual void Initialize(ShaderProgram transformShader)
        {
            _transformShader = transformShader;
            _transformation = Pipeline.Pipeline.GetTransformation(this);

            GL.GenBuffers(1, out _iboIndex);//генерируем индификатор для индексов вершин
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _iboIndex);//начинаем работу с буфером (1. тип буфера (ElementArrayBuffer-для ссылок на данные),
                                                                      //2. индификатор буфера, с которым будем работать)
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indecies.Length*sizeof(uint), Indecies, BufferUsageHint.StaticDraw); ;//передаем данные из массива(1. тип буфера
                                                                                                                                 //2. размер в байтах, который необходимо выделить для массива
                                                                                                                                 //(количество элементов * тип данных)
                                                                                                                                 //3. передаем сами данные
                                                                                                                                 //4. тип доступа)

            _vboVerticesIndex = GL.GenBuffer();//генерируем индификатор для создаваемого буфера
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboVerticesIndex);//начинаем работу с буфером (1. тип буфера (ArrayBuffer-для хранения данных),
                                                                       //2. индификатор буфера, с которым будем работать)
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length*sizeof(float), Vertices, BufferUsageHint.StaticDraw);//передаем данные из массива(1. тип буфера
                                                                                                                         //2. размер в байтах, который необходимо выделить для массива
                                                                                                                         //(количество элементов * тип данных)
                                                                                                                         //3. передаем сами данные
                                                                                                                         //4. тип доступа)

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);//отключаем буфер, ставим указатель на 0-ой буфер

            _vboColorsIndex = GL.GenBuffer();//генерируем индификатор для создаваемого буфера
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboColorsIndex);//начинаем работу с буфером (1. тип буфера (ArrayBuffer-для хранения данных),
                                                                     //2. индификатор буфера, с которым будем работать)
            GL.BufferData(BufferTarget.ArrayBuffer, Colors.Length*sizeof(float), Colors, BufferUsageHint.StaticDraw);//передаем данные из массива(1. тип буфера
                                                                                                                     //2. размер в байтах, который необходимо выделить для массива
                                                                                                                     //(количество элементов * тип данных)
                                                                                                                     //3. передаем сами данные
                                                                                                                     //4. тип доступа(где будет храниться: в динамической или статической памяти))

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);//отключаем буфер
        }

        public virtual void Update()
        {
            _transformShader.SetMatrix4("transform", _transformation);



            GL.EnableClientState(ArrayCap.VertexArray);//активируем возможность работы с массивом вершин(передаем массив, с которым работаем)
            GL.EnableClientState(ArrayCap.ColorArray);//активируем возможность работы с массивом цветов

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboVerticesIndex);//начинаем работу с буфером (1. тип буфера (ArrayBuffer-для хранения данных),
                                                                       //2. индификатор буфера, с которым будем работать)
            GL.VertexPointer(3, VertexPointerType.Float, 0, 0);//описание точки(1. сколько чисел надо считать, чтобы получить данные об одной вершине,
                                                               //2. тип данных,
                                                               //3. шаг смещения при считывании данных
                                                               //4. массив данных(берем данные из видеокарты))


            //GL.BindBuffer(BufferTarget.ArrayBuffer, _vboColorsIndex);//начинаем работу с буфером (1. тип буфера (ArrayBuffer-для хранения данных),
            //2. индификатор буфера, с которым будем работать)
            //GL.ColorPointer(4, ColorPointerType.Float, 0, 0);//описание цвета точки

            //GL.DrawArrays(RenderType, 0, VerticesCount);//отрисовка(1. тип примитива, который создаем, 2. с какой вершины начать отрисовку,
            //3. сколько вершин будет считываться для создания примитивов)

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _iboIndex);//начинаем работу с буфером (1. тип буфера (ArrayBuffer-для хранения данных),
                                                                      //2. индификатор буфера, с которым будем работать)
            GL.DrawElements(RenderType, VerticesCount, DrawElementsType.UnsignedInt, 0);//отрисовка(1. тип примитива, который создаем, 2. сколько вершин будет считываться для создания примитивов,
                                                                                        //3. указываем тип индексов, 4. локация, где находятся индексы)

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);//отключаем буфер
            GL.DisableClientState(ArrayCap.VertexArray);//отключаем возможность работы с массивом
            GL.DisableClientState(ArrayCap.ColorArray);
        }

        ~RendererComponent()
        {
            GL.DeleteBuffer(_vboVerticesIndex);//удаляем буфер
            GL.DeleteBuffer(_vboColorsIndex);
        }
    }
}

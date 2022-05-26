using OpenGL.Shader;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace OpenGL.Renderer
{
    public abstract class RendererComponent
    {
        protected ShaderProgram _transformShader;//шейдер для отображения
        protected Texture.Texture _texture;
        protected int _vboVerticesIndex;//индификатор, в котором хранятся вершины
        protected int _vboColorsIndex;//индификатор, в котором хранятся цвета
        protected uint _iboIndex;//индификатор, в котором хранятся индексы вершин

        protected Vector3 _position = new Vector3(0, 0, 2);//позиция объекта
        protected Vector3 _rotation = new Vector3(0,0,0);//поворот объекта
        protected Vector3 _scale = Vector3.One;//масштаб  объекта
        protected Matrix4 _transformation = Matrix4.Identity;//финальное положение объекта

        public abstract Vertice[] Vertices { get; set; }
        public virtual float[] Colors { get; set; }
        public abstract uint[] Indecies { get; set; }

        public Vector3 Position { get { return _position; } set { _position=value; Transformation = Pipeline.Pipeline.GetTransformation(this); } }
        public Vector3 Rotation { get { return _rotation; } set { _rotation=value; Transformation = Pipeline.Pipeline.GetTransformation(this); } }
        public Vector3 Scale { get { return _scale; } set { _scale=value; Transformation = Pipeline.Pipeline.GetTransformation(this); } }
        public Matrix4 Transformation { get { return _transformation; } set { _transformation=value; } }

        protected virtual PrimitiveType RenderType { get; set; } = PrimitiveType.TriangleStrip;

        public virtual int VerticesCount { get { return Indecies.Length; } }

        public RendererComponent()
        {

        }

        public RendererComponent(Vector3 position,Vector3 scale)
        {
            _position = position;
            _scale = scale;
        }

        public void UpdateTransformation()
        {
            _transformation = Pipeline.Pipeline.GetTransformation(this);
        }

        public virtual void Initialize(ShaderProgram transformShader,Texture.Texture texture)
        {
            _transformShader = transformShader;
            _texture = texture;
            UpdateTransformation();

            Pipeline.Pipeline.CalcNormals(Indecies, Vertices);

            GL.GenBuffers(1, out _iboIndex);//генерируем индификатор для индексов вершин
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _iboIndex);//начинаем работу с буфером (1. тип буфера (ElementArrayBuffer-для ссылок на данные),
                                                                      //2. индификатор буфера, с которым будем работать)
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indecies.Length*sizeof(uint), Indecies, BufferUsageHint.StaticDraw);//передаем данные из массива(1. тип буфера
                                                                                                                                 //2. размер в байтах, который необходимо выделить для массива
                                                                                                                                 //(количество элементов * тип данных)
                                                                                                                                 //3. передаем сами данные
                                                                                                                                 //4. тип доступа(где будет храниться: в динамической или статической памяти))

            _vboVerticesIndex = GL.GenBuffer();//генерируем индификатор для создаваемого буфера
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboVerticesIndex);//начинаем работу с буфером (1. тип буфера (ArrayBuffer-для хранения данных),
                                                                       //2. индификатор буфера, с которым будем работать)
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length*sizeof(float)*8, Vertices, BufferUsageHint.StaticDraw);//передаем данные из массива(1. тип буфера
                                                                                                                         //2. размер в байтах, который необходимо выделить для массива
                                                                                                                         //(количество элементов * тип данных)
                                                                                                                         //3. передаем сами данные, 4. тип доступа)

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);//отключаем буфер, ставим указатель на 0-ой буфер
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);//отключаем буфер
        }

        public virtual void Update()
        {
            _transformShader.SetMatrix4("transform", _transformation);//связываем программный код с кодом шейдера   

            GL.EnableVertexAttribArray(0);//активируем возможность работы с массивом
            GL.EnableVertexAttribArray(1);//активируем возможность работы с массивом
            GL.EnableVertexAttribArray(2);//активируем возможность работы с массивом

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboVerticesIndex);//начинаем работу с буфером (1. тип буфера (ArrayBuffer-для хранения данных), 2. индификатор буфера, с которым будем работать)
            GL.VertexAttribPointer(0,3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);//описание точки
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));//описание точки
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));//описание точки

            _texture.Enable(TextureUnit.Texture0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _iboIndex);//начинаем работу с буфером (1. тип буфера, 2. индификатор буфера, с которым будем работать)
            GL.DrawElements(RenderType, VerticesCount, DrawElementsType.UnsignedInt, 0);

            GL.DisableVertexAttribArray(0);//отключаем возможность работы с массивом
            GL.DisableVertexAttribArray(1);//отключаем возможность работы с массивом
            GL.DisableVertexAttribArray(2);//отключаем возможность работы с массивом
        }

        ~RendererComponent()
        {
            GL.DeleteBuffer(_vboVerticesIndex);//удаляем буфер
            GL.DeleteBuffer(_vboColorsIndex);
        }
    }

    public struct Vertice
    {
        public Vector3 Pos;
        public Vector2 UV;
        public Vector3 Normal = Vector3.Zero;

        public Vertice(Vector3 pos, Vector2 uv, Vector3 normal)
        {
            Pos = pos;
            UV = uv;
            Normal = normal;
        }
    }
}

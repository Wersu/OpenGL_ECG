    using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace OpenGL.Shader
{
    public class ShaderProgram
    {
        private int _program = 0;//индификатор шейдерной программы

        public int Program { get { return _program; } }
        public ShaderProgram(List<ShaderPath> paths)
        {
            Initialize(paths);
        }

        private void Initialize(List<ShaderPath> paths)
        {
            List<int> shadersID = new List<int>();

            foreach (var item in paths)
                shadersID.Add(CreateShader(item));//для каждый шейдер добавляем в список индификатор создаваемого шейдера 

            _program = GL.CreateProgram();//создаем шейдерную программу

            foreach (var item in shadersID)
                GL.AttachShader(_program, item);//каждый шейдер прикрепляем к шейдерной программе(1. индитификатор программы, 2. индитификатор шейдера)

            GL.LinkProgram(_program);//компилируем шейдерную программу
            GL.GetProgram(_program, GetProgramParameterName.LinkStatus, out int statusCode);//проверяем скомпилировалась ли программа(1. индификатор программы, 2.Состояние компиляции, 3. результат компиляции)
            if (statusCode != (int)All.True)//если произошла ошибка(All.True - перечисление true)   
            {
                string infoLog = GL.GetProgramInfoLog(_program);//информация об ошибке
                throw new Exception($"Program [{_program}] link error \n\n{infoLog}");
            }

            foreach (var item in shadersID)
                DeleteShader(item);//удаляем шейдеры, тк они уже хранятся в памяти
        }

        public void Enable() => GL.UseProgram(_program);//активируем программу шейдера

        public void Disable() => GL.UseProgram(0);//деактивируем программу шейдера

        public void SetMatrix4(string name, Matrix4 value)
        {
            int location = GL.GetUniformLocation(_program, name);//индификатор location данной Uniform(указываем имя переменной в коде шейдера)
            GL.UniformMatrix4(location, true, ref value);//свзываем программный код с кодом шейдера
        }

        public void SetVector3(int loc, Vector3 value)
        {
            GL.Uniform3(loc, value.X,value.Y,value.Z);
        }

        public void SetFloat(int loc,float value)
        {
            GL.Uniform1(loc, value);
        }

        private void DeleteProgram() => DeleteProgram(_program);

        private void DeleteShader(int shaderID) => DeleteShader(_program, shaderID);

        private static void DeleteProgram(int id) => GL.DeleteProgram(id);

        private static int CreateShader(ShaderPath path)
        {
            string shaderCode = File.ReadAllText(path.Path.ToString());//программный код шейдера
            int shaderID = GL.CreateShader(path.Type);//генерируем индификатор (передаем тип создаваемого шейдера) 

            GL.ShaderSource(shaderID, shaderCode);//прикрепляем наш код к шейдеру(1. индификатор, к которому прикрепляем, 2. код шейдера)
            GL.CompileShader(shaderID);//компилируем программный код шейдера(индификатор, который должен скомпилироваться)

            GL.GetShader(shaderID, ShaderParameter.CompileStatus, out int statusCode);//проверяем скомпилировался ли шейдер(1. мндификатор шейдера, 2.Состояние компиляции, 3. результат компиляции)
            if (statusCode != (int)All.True)//если произошла ошибка(All.True - перечисление true)   
            {
                string infoLog = GL.GetShaderInfoLog(shaderID);//информация об ошибке
                throw new Exception($"Shader [{shaderID}] compile error \n\n{infoLog}");
            }

            return shaderID;
        }

        private static void DeleteShader(int programID, int shaderID)
        {
            GL.DetachShader(programID, shaderID);//отсоединяем от программы шейдер
            GL.DeleteShader(shaderID);//удаляем шейдер
        }

        ~ShaderProgram()
        {
            DeleteProgram();
        }
    }
}

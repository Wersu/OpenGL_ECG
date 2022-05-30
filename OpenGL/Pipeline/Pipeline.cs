using OpenGL.Renderer;
using OpenTK.Mathematics;

namespace OpenGL.Pipeline
{
    public static class Pipeline
    {
        private static Vector3 worldScale = Vector3.One;
        private static Vector3 worldPosition = Vector3.Zero;

        private static float ToRadian(float value) => (float)(value*Math.PI/180f);
        private static float ToDegree(float value) => (float)(value*180f/Math.PI);
        private static Matrix4 GetScaleTrans(Vector3 scale)
        {
            Matrix4 result = new Matrix4();

            result.Row0.X = scale.X; result.Row0.Y = 0; result.Row0.Z = 0; result.Row0.W = 0;
            result.Row1.X = 0; result.Row1.Y = scale.Y; result.Row1.Z = 0; result.Row1.W = 0;
            result.Row2.X = 0; result.Row2.Y = 0; result.Row2.Z = scale.Z; result.Row2.W = 0;
            result.Row3.X = 0; result.Row3.Y = 0; result.Row3.Z = 0; result.Row3.W = 1;


            return result;
        }

        private static Matrix4 GetRotateTrans(Vector3 rot)
        {
            float x = ToRadian(rot.X);
            float y = ToRadian(rot.Y);
            float z = ToRadian(rot.Z);

            Matrix4 rx, ry, rz;

            rx.Row0.X = 1; rx.Row0.Y = 0; rx.Row0.Z = 0; rx.Row0.W = 0;
            rx.Row1.X = 0; rx.Row1.Y = (float)Math.Cos(x); rx.Row1.Z = -(float)Math.Sin(x); rx.Row1.W = 0;
            rx.Row2.X = 0; rx.Row2.Y = (float)Math.Sin(x); rx.Row2.Z = (float)Math.Cos(x); rx.Row2.W = 0;
            rx.Row3.X = 0; rx.Row3.Y = 0; rx.Row3.Z = 0; rx.Row3.W = 1;

            ry.Row0.X = (float)Math.Cos(y); ry.Row0.Y = 0; ry.Row0.Z = -(float)Math.Sin(y); ry.Row0.W = 0;
            ry.Row1.X = 0; ry.Row1.Y = 1; ry.Row1.Z = 0; ry.Row1.W = 0;
            ry.Row2.X = (float)Math.Sin(y); ry.Row2.Y = 0; ry.Row2.Z = (float)Math.Cos(y); ry.Row2.W = 0;
            ry.Row3.X = 0; ry.Row3.Y = 0; ry.Row3.Z = 0; ry.Row3.W = 1;

            rz.Row0.X = (float)Math.Cos(z); rz.Row0.Y = -(float)Math.Sin(z); rz.Row0.Z = 0; rz.Row0.W = 0;
            rz.Row1.X = (float)Math.Sin(z); rz.Row1.Y = (float)Math.Cos(z); rz.Row1.Z = 0; rz.Row1.W = 0;
            rz.Row2.X = 0; rz.Row2.Y = 0; rz.Row2.Z = 1; rz.Row2.W = 0;
            rz.Row3.X = 0; rz.Row3.Y = 0; rz.Row3.Z = 0; rz.Row3.W = 1;

            return rz*ry*rx;
        }

        private static Matrix4 GetTranslationTrans(Vector3 vec)
        {
            Matrix4 result = new Matrix4();

            result.Row0.X = 1; result.Row0.Y = 0; result.Row0.Z = 0; result.Row0.W = vec.X;
            result.Row1.X = 0; result.Row1.Y = 1; result.Row1.Z = 0; result.Row1.W = vec.Y;
            result.Row2.X = 0; result.Row2.Y = 0; result.Row2.Z = 1; result.Row2.W = vec.Z;
            result.Row3.X = 0; result.Row3.Y = 0; result.Row3.Z = 0; result.Row3.W = 1;

            return result;
        }

        private static Matrix4 GetPerspectiveProj()
        {
            Matrix4 result = new Matrix4();

            float ar = Camera.Camera.Width/Camera.Camera.Height;
            float zNear = Camera.Camera.Z_NEAR;
            float zFar = Camera.Camera.Z_FAR;
            float zRange = zNear-zFar;
            float tanHalfFOV = (float)Math.Tan(ToRadian(Camera.Camera.FOV/2f));

            result.Row0.X = 1f / (tanHalfFOV*ar); result.Row0.Y = 0; result.Row0.Z = 0; result.Row0.W = 0;
            result.Row1.X = 0; result.Row1.Y = 1f/tanHalfFOV; result.Row1.Z = 0; result.Row1.W = 0;
            result.Row2.X = 0; result.Row2.Y = 0; result.Row2.Z = (-zNear-zFar)/zRange; result.Row2.W = 2*zFar*zNear/zRange;
            result.Row3.X = 0; result.Row3.Y = 0; result.Row3.Z = 1; result.Row3.W = 0;

            return result;
        }

        public static Matrix4 GetCameraTrans()
        {
            Matrix4 result = new Matrix4();

            Vector3 Target = Vector3.Normalize(Camera.Camera.Rotation);
            Vector3 Up = Vector3.Normalize(Camera.Camera.Up);
            Up = Vector3.Cross(Target, Up);
            Vector3 V = Vector3.Cross(Target, Up);

            result.Row0.X = Up.X; result.Row0.Y = Up.Y; result.Row0.Z = Up.Z; result.Row0.W = 0;
            result.Row1.X = V.X; result.Row1.Y = V.Y; result.Row1.Z = V.Z; result.Row1.W = 0;
            result.Row2.X = Target.X; result.Row2.Y = Target.Y; result.Row2.Z = Target.Z; result.Row2.W = 0;
            result.Row3.X = 0; result.Row3.Y = 0; result.Row3.Z = 0; result.Row3.W = 1;

            return result;
        }

        public static Matrix4 GetTransformation(RendererComponent component)
        {
            return GetTransformation(component.Position, component.Scale, component.Rotation);
        }

        public static Matrix4 GetTransformation(Vector3 pos, Vector3 scale, Vector3 rotation)
        {
            Matrix4 CameraRotationTrans = GetCameraTrans();
            Matrix4 CameraTranslationTrans = GetTranslationTrans(Camera.Camera.Position);
            Matrix4 TranslationTrans = GetTranslationTrans(pos);
            Matrix4 RotationTrans = GetRotateTrans(rotation);
            Matrix4 ScaleTrans = GetScaleTrans(scale);
            Matrix4 PerspectiveProj = GetPerspectiveProj();

            return PerspectiveProj * CameraRotationTrans * CameraTranslationTrans * TranslationTrans * RotationTrans * ScaleTrans;
        }

        public static Matrix4 GetWorldTransformation()
        {
            Matrix4 TranslationTrans = GetTranslationTrans(worldPosition);
            Matrix4 RotationTrans = GetRotateTrans(Vector3.Zero);
            Matrix4 ScaleTrans = GetScaleTrans(worldScale);

            return TranslationTrans * RotationTrans * ScaleTrans;
        }

        public static void CalcNormals(uint[] Indencies, Vertice[] Vertices)
        {
            for (int i = 0; i<Indencies.Length; i += 3)
            {
                uint Index0 = Indencies[i];
                uint Index1 = Indencies[i+1];
                uint Index2 = Indencies[i+2];
                Vector3 v1 = Vertices[Index1].Pos - Vertices[Index0].Pos;
                Vector3 v2 = Vertices[Index2].Pos - Vertices[Index0].Pos;
                Vector3 Normal = Vector3.Cross(v1, v2);
                Normal.Normalize();

                Vertices[Index0].Normal = Vertices[Index0].Normal + Normal;
                Vertices[Index1].Normal = Vertices[Index1].Normal + Normal;
                Vertices[Index2].Normal = Vertices[Index2].Normal + Normal;
            }

            for (   int i = 0; i<Vertices.Length; i++)
            {
                Vertices[i].Normal.Normalize();
            }
        }

        public static void SetScale(Vector3 scale)
        {
            worldScale = scale;
        }

        public static void SetPosition(Vector3 pos)
        {
            worldPosition = pos;
        }
    }
}

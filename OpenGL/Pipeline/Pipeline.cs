using OpenGL.Renderer;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL.Pipeline
{
    public static class Pipeline
    {
        private static float ToRadian(float value) => (float)(value*Math.PI/180f);
        private static float ToDegree(float value) => (float)(value*180f/Math.PI);
        private static Matrix4 GetScaleTrans(RendererComponent component)
        {
            Matrix4 result = new Matrix4();

            result.Row0.X = component.Scale.X;  result.Row0.Y = 0;                  result.Row0.Z = 0;                  result.Row0.W = 0;
            result.Row1.X = 0;                  result.Row1.Y = component.Scale.Y;  result.Row1.Z = 0;                  result.Row1.W = 0;
            result.Row2.X = 0;                  result.Row2.Y = 0;                  result.Row2.Z = component.Scale.Z;  result.Row2.W = 0;
            result.Row3.X = 0;                  result.Row3.Y = 0;                  result.Row3.Z = 0;                  result.Row3.W = 1;


            return result;
        }

        private static Matrix4 GetRotateTrans(RendererComponent component)
        {
            float x = ToRadian(component.Rotation.X);
            float y = ToRadian(component.Rotation.Y);
            float z = ToRadian(component.Rotation.Z);

            Matrix4 rx, ry, rz;

                rx.Row0.X = 1;                  rx.Row0.Y = 0;                  rx.Row0.Z = 0;                  rx.Row0.W = 0;
                rx.Row1.X = 0;                  rx.Row1.Y = (float)Math.Cos(x); rx.Row1.Z = -(float)Math.Sin(x);rx.Row1.W = 0;
                rx.Row2.X = 0;                  rx.Row2.Y = (float)Math.Sin(x); rx.Row2.Z = (float)Math.Cos(x); rx.Row2.W = 0;
                rx.Row3.X = 0;                  rx.Row3.Y = 0;                  rx.Row3.Z = 0;                  rx.Row3.W = 1;

                ry.Row0.X = (float)Math.Cos(y); ry.Row0.Y = 0;                  ry.Row0.Z = -(float)Math.Sin(y);ry.Row0.W = 0;
                ry.Row1.X = 0;                  ry.Row1.Y = 1;                  ry.Row1.Z = 0;                  ry.Row1.W = 0;
                ry.Row2.X = (float)Math.Sin(y); ry.Row2.Y = 0;                  ry.Row2.Z = (float)Math.Cos(y); ry.Row2.W = 0;
                ry.Row3.X = 0;                  ry.Row3.Y = 0;                  ry.Row3.Z = 0;                  ry.Row3.W = 1;

                rz.Row0.X = (float)Math.Cos(z); rz.Row0.Y = -(float)Math.Sin(z);rz.Row0.Z = 0;                  rz.Row0.W = 0;
                rz.Row1.X = (float)Math.Sin(z); rz.Row1.Y = (float)Math.Cos(z); rz.Row1.Z = 0;                  rz.Row1.W = 0;
                rz.Row2.X = 0;                  rz.Row2.Y = 0;                  rz.Row2.Z = 1;                  rz.Row2.W = 0;
                rz.Row3.X = 0;                  rz.Row3.Y = 0;                  rz.Row3.Z = 0;                  rz.Row3.W = 1;

            return rz*ry*rx;
        }

        private static Matrix4 GetTranslationTrans(RendererComponent component)
        {
            Matrix4 result = new Matrix4();

                result.Row0.X = 1;                  result.Row0.Y = 0;                  result.Row0.Z = 0;                  result.Row0.W = component.Position.X;
                result.Row1.X = 0;                  result.Row1.Y = 1;                  result.Row1.Z = 0;                  result.Row1.W = component.Position.Y;
                result.Row2.X = 0;                  result.Row2.Y = 0;                  result.Row2.Z = 1;                  result.Row2.W = component.Position.Z;
                result.Row3.X = 0;                  result.Row3.Y = 0;                  result.Row3.Z = 0;                  result.Row3.W = 1;

            return result;
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

            float ar = 800f/800f;
            float zNear = 1f;
            float zFar = 1000f;
            float zRange = zNear-zFar;
            float tanHalfFOV = (float)Math.Tan(ToRadian(90f/2f));

                result.Row0.X = 1f / (tanHalfFOV*ar); result.Row0.Y = 0;                  result.Row0.Z = 0;                    result.Row0.W = 0;
                result.Row1.X = 0;                    result.Row1.Y = 1f/tanHalfFOV;      result.Row1.Z = 0;                    result.Row1.W = 0;
                result.Row2.X = 0;                    result.Row2.Y = 0;                  result.Row2.Z = (-zNear-zFar)/zRange; result.Row2.W = 2*zFar*zNear/zRange;
                result.Row3.X = 0;                    result.Row3.Y = 0;                  result.Row3.Z = 1;                    result.Row3.W = 0;

            return result;
        }

        private static Matrix4 GetCameraTrans()
        {
            Matrix4 result = new Matrix4();

            Vector3 Target = Vector3.Normalize(new Vector3(0.00f, 0.0f, 1.0f));
            Vector3 Up = Vector3.Normalize(new Vector3(0, 1, 0));
            Up = Vector3.Cross(Target, Up);
            Vector3 V = Vector3.Cross(Target, Up);

            result.Row0.X = Up.X;       result.Row0.Y = Up.Y;       result.Row0.Z = Up.Z;       result.Row0.W = 0;
            result.Row1.X = V.X;        result.Row1.Y = V.Y;        result.Row1.Z = V.Z;        result.Row1.W = 0;
            result.Row2.X = Target.X;   result.Row2.Y = Target.Y;   result.Row2.Z = Target.Z;   result.Row2.W = 0;
            result.Row3.X = 0;          result.Row3.Y = 0;          result.Row3.Z = 0;          result.Row3.W = 1;

            return result;
        }

        public static Matrix4 GetTransformation(RendererComponent component)
        {
            Matrix4 CameraRotationTrans = GetCameraTrans();
            Matrix4 CameraTranslationTrans = GetTranslationTrans(Vector3.Zero);
            Matrix4 TranslationTrans = GetTranslationTrans(component);
            Matrix4 RotationTrans = GetRotateTrans(component);
            Matrix4 ScaleTrans = GetScaleTrans(component);
            Matrix4 PerspectiveProj = GetPerspectiveProj();

            return PerspectiveProj * TranslationTrans * RotationTrans * ScaleTrans;
        }
    }
}

using OpenTK.Windowing.Common;

namespace OpenGL.Updatable
{
    internal interface IFrameUpdatable
    {
        void Update(FrameEventArgs args);
    }
}

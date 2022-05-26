﻿using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL.Updatable
{
    internal interface IFrameUpdatable
    {
        void Update(FrameEventArgs args);
    }
}

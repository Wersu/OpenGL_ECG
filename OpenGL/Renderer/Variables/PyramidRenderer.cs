using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL.Renderer.Variables
{
    internal class PyramidRenderer : RendererComponent
    {
        private float[] _vertices = {
             -1,0,0,
             0,0,-1,
             1,0,0,
             0,1,0,
        };
                
        private float[] _colors = {
            1,0,0,1,
            0,0,1,1,
            0,1,0,1,     

            1,0,0,1,
            1,1,1,1,
            0,0,1,1,

            0,0,1,1,
            1,1,1,1,
            0,1,0,1,

            0,1,0,1,
            1,1,1,1,
            1,0,0,1,
        };

        private uint[] _indencies = { 
            0,1,2,
            0,3,1,
            1,3,2,
            2,3,0,
        };

        public override float[] Vertices { get { return _vertices; } set { _vertices = value; } }
        public override float[] Colors { get { return _colors; } set { _colors = value; } }
        public override uint[] Indecies { get { return _indencies; } set { _indencies = value; } }

        protected override PrimitiveType RenderType { get; set; } = PrimitiveType.Triangles;
    }
}

using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL.Renderer.Variables
{
    internal class CubeRenderer : RendererComponent
    {
        private Vertice[] _vertices = {
             new Vertice(new Vector3(-1,0,1),new Vector2(  0,0),Vector3.Zero),//0
             new Vertice(new Vector3(-1,0,-1),new Vector2( 1,0),Vector3.Zero),//1
             new Vertice(new Vector3(-1,2,1),new Vector2(  0,1),Vector3.Zero),//2
             new Vertice(new Vector3(-1,2,-1),new Vector2( 1,1),Vector3.Zero),//3
             new Vertice(new Vector3(1,0,1),new Vector2(   1,0),Vector3.Zero),//4
             new Vertice(new Vector3(1,0,-1),new Vector2(  0,0),Vector3.Zero),//5
             new Vertice(new Vector3(1,2,1),new Vector2(   1,1),Vector3.Zero),//6
             new Vertice(new Vector3(1,2,-1),new Vector2(  0,1),Vector3.Zero),//7

             new Vertice(new Vector3(-1,0,-1),new Vector2( 0,0),Vector3.Zero),//8 - 1
             new Vertice(new Vector3(-1,0,1),new Vector2(  0,1),Vector3.Zero),//9 - 0
             new Vertice(new Vector3(1,0,1),new Vector2(   1,1),Vector3.Zero),//10 - 4
             new Vertice(new Vector3(1,0,-1),new Vector2(  1,0),Vector3.Zero),//11 - 5

             new Vertice(new Vector3(-1,2,1),new Vector2(  0,0),Vector3.Zero),//12 - 2
             new Vertice(new Vector3(-1,2,-1),new Vector2( 0,1),Vector3.Zero),//13 - 3
             new Vertice(new Vector3(1,2,-1),new Vector2(  1,1),Vector3.Zero),//14 - 7
             new Vertice(new Vector3(1,2,1),new Vector2(   1,0),Vector3.Zero),//15 - 6
        };

        private uint[] _indencies = {
            0,2,6,4,
            4,6,7,5,
            5,7,3,1,
            1,3,2,0,

            8,9,10,11,
            12,13,14,15,

            //2,3,7,6,
            //1,0,4,5,


            /*0,2,6,
            0,6,4, 

            4,6,7,
            4,7,5,

            1,3,2,
            1,2,0,

            5,7,3,
            5,3,1,

            3,6,2,
            3,7,6,

            1,0,4,
            1,4,5,*/
        };

        public override Vertice[] Vertices { get { return _vertices; } set { _vertices = value; } }
        public override uint[] Indecies { get { return _indencies; } set { _indencies = value; } }

        protected override PrimitiveType RenderType { get; set; } = PrimitiveType.Quads;

        public CubeRenderer() : base() { }

        public CubeRenderer(Vector3 position, Vector3 scale) : base(position, scale) { }
    }
}

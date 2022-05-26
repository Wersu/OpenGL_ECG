using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL
{
    internal class GameComponent
    {
        protected Game _game;

        public virtual void Initialize(Game game)
        {
            _game = game;
        }
    }
}

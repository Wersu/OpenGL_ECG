namespace OpenGL
{
    public class GameComponent
    {
        protected Game _game;

        public virtual void Initialize(Game game)
        {
            _game = game;
        }
    }
}

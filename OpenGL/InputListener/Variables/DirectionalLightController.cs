using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace OpenGL.InputListener.Variables
{
    internal class DirectionalLightController : GameComponent, IKeyListener
    {
        private const float CHANGE_SPEED = 0.01f;

        private List<Lightning.Variables.DirectionalLight> lights;
        private List<Lightning.Variables.PointLight> pointLights;
        public override void Initialize(Game game)
        {
            base.Initialize(game);

            lights = game.Lights.Where(x=>x is Lightning.Variables.DirectionalLight).Cast<Lightning.Variables.DirectionalLight>().ToList();
            pointLights = game.Lights.Where(x => x is Lightning.Variables.PointLight).Cast<Lightning.Variables.PointLight>().ToList();
        }
        public void KeyPressed(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.Q))
                lights.ForEach(x => x.light.AmbientIntensity += CHANGE_SPEED);
            if(state.IsKeyDown(Keys.E))
                lights.ForEach(x => x.light.AmbientIntensity -= CHANGE_SPEED);

            if (state.IsKeyDown(Keys.Up))
            {
                pointLights.ForEach(x => x.Position += new OpenTK.Mathematics.Vector3(0, -0.1f, 0));
            }
            if (state.IsKeyDown(Keys.Down))
            {
                pointLights.ForEach(x => x.Position += new OpenTK.Mathematics.Vector3(0, 0.1f, 0));
            }
        }
    }
}

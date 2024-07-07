using Mfx.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mfx.Samples.FlyingObject
{
    public class FlyingObjectGame : MfxGame
    {
        public FlyingObjectGame()
        {
            FirstScene = AddScene<FlyingObjectMainScene>();
        }
    }
}

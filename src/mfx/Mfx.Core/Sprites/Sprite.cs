using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mfx.Core.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mfx.Core.Sprites
{
    public class Sprite(IScene scene, Texture2D? texture, float x, float y) : VisibleComponent(scene, texture, x, y)
    {
        protected override void ExecuteDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Texture is not null)
            {
                spriteBatch.Draw(Texture, new Vector2(X, Y), Color.White);
            }
        }
    }
}

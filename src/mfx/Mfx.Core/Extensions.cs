using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mfx.Core.Scenes;
using Mfx.Core.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mfx.Core
{
    public static class Extensions
    {
        public static Sprite ToSprite(this Texture2D texture, IScene scene, float x, float y, int layer = 0,
            Action<GameTime, SpriteBatch, Texture2D>? drawAction = null) =>
            new CustomWrappingSprite(scene, texture, x, y, drawAction)
            {
                Layer = layer
            };

        private sealed class CustomWrappingSprite(
            IScene scene,
            Texture2D texture,
            float x,
            float y,
            Action<GameTime, SpriteBatch, Texture2D>? drawAction)
            : Sprite(scene, texture, x, y)
        {
            protected override void ExecuteDraw(GameTime gameTime, SpriteBatch spriteBatch)
            {
                if (drawAction is not null && Texture is not null)
                {
                    drawAction(gameTime, spriteBatch, Texture);
                }
                else
                {
                    base.ExecuteDraw(gameTime, spriteBatch);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mfx.Core;
using Mfx.Core.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mfx.Samples.ElementsDemo
{
    internal sealed class NewGameScene(MfxGame game, string name) : Scene(game, name)
    {
        private SpriteFont? _font;

        public override void Load(ContentManager contentManager)
        {
            _font = contentManager.Load<SpriteFont>("arial");
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                Game.Transit("ElementsDemoScene");
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
           
            base.Draw(gameTime, spriteBatch);
            //spriteBatch.Begin();
            spriteBatch.DrawString(_font, "This is the game scene. Press ENTER to go back.", Vector2.Zero, Color.Yellow);
            //spriteBatch.End();
        }
    }
}

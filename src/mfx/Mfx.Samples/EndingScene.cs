using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mfx.Core;
using Mfx.Core.Elements;
using Mfx.Core.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mfx.Samples
{
    internal sealed class EndingScene(MfxGame game, string name) : Scene(game, name, Color.Black)
    {
        private SpriteFont? _font;

        public override void Load(ContentManager contentManager)
        {
            _font = contentManager.Load<SpriteFont>("arial");
            Add(new Label("Press ENTER to exit.", this, _font, Color.MediumVioletRed));
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                Game.Exit();
            }

            base.Update(gameTime);
        }
    }
}

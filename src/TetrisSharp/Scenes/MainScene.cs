using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mfx.Core;
using Mfx.Core.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace TetrisSharp.Scenes
{
    internal sealed class MainScene(MfxGame game, string name) : Scene(game, name, Color.Gray)
    {
        public override void Load(ContentManager contentManager)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            var keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Escape))
            {
                Game.Transit<TitleScene>();
            }

            base.Update(gameTime);
        }
    }
}

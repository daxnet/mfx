using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mfx.Core;
using Mfx.Core.Elements;
using Mfx.Core.Elements.Menus;
using Mfx.Core.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TetrisSharp.Scenes
{
    internal sealed class TitleScene(TetrisGame game, string name) : Scene(game, name, Color.Black)
    {
        private Menu? _menu;
        private SpriteFont? _menuFont;

        public override void Load(ContentManager contentManager)
        {
            var backgroundImageTexture = contentManager.Load<Texture2D>("images\\title");
            Add(new Image(this, backgroundImageTexture));

            _menuFont = contentManager.Load<SpriteFont>("fonts\\menu");
            _menu = new Menu(this, _menuFont, [
                new MenuItem("mnuNewGame", "New Game"),
                new MenuItem("mnuContinue", "Continue") { Enabled = false },
                new MenuItem("mnuLoad", "Load") { Enabled = false },
                new MenuItem("mnuControllerOptions", "Controller Settings"),
                new MenuItem("mnuExit", "Exit")
            ], 550, 230, Color.Yellow, Color.Brown, Color.Gray, alignment: Menu.Alignment.Right)
            {
                Layer = int.MaxValue
            };

            Add(_menu);

            SubscribeMessages();
        }

        public override void Enter(object? args = null)
        {
            var continueMenuItem = _menu?.GetMenuItem("mnuContinue");
            if (Game is TetrisGame tg && continueMenuItem is not null)
            {
                continueMenuItem.Enabled = tg.CanContinue;
            }
        }

        public override void Update(GameTime gameTime)
        {
            //if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            //{
            //    Game.Exit();
            //}
            base.Update(gameTime);
        }

        public override void Leave()
        {
            Mouse.SetCursor(MouseCursor.Arrow);
        }

        private void SubscribeMessages()
        {
            Subscribe<MenuItemClickedMessage>((_, message) =>
            {
                switch (message.MenuItemName)
                {
                    case "mnuNewGame":
                        Game.Transit<GameScene>();
                        break;
                    case "mnuContinue":
                        Game.Transit<GameScene>();
                        break;
                    case "mnuExit":
                        Game.Exit();
                        break;
                }
            });
        }
    }
}

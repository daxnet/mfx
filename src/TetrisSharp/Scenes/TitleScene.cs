using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mfx.Core;
using Mfx.Core.Elements;
using Mfx.Core.Elements.Menus;
using Mfx.Core.Scenes;
using Mfx.Core.Sounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TetrisSharp.Scenes
{
    internal sealed class TitleScene(TetrisGame game, string name) : Scene(game, name, Color.Black)
    {
        private Menu? _menu;
        private SpriteFont? _menuFont;
        private BackgroundMusic? _bgm;
        private SoundEffect? _bgmEffect;
        private bool _disposed;

        public override void Load(ContentManager contentManager)
        {
            // Background music
            _bgmEffect = contentManager.Load<SoundEffect>(@"sounds\opening");
            _bgm = new([_bgmEffect], .2f);

            // Background images
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
                Layer = int.MaxValue // Put the menu on top
            };

            Add(_menu);
            Add(_bgm);

            SubscribeMessages();
        }

        public override void Enter(object? args = null)
        {
            var continueMenuItem = _menu?.GetMenuItem("mnuContinue");
            if (continueMenuItem is not null)
            {
                continueMenuItem.Enabled = GameAs<TetrisGame>().CanContinue;
            }

            _bgm?.Play();
        }

        public override void Leave()
        {
            Mouse.SetCursor(MouseCursor.Arrow);
            _bgm?.Stop();
        }

        private void SubscribeMessages()
        {
            Subscribe<MenuItemClickedMessage>((_, message) =>
            {
                switch (message.MenuItemName)
                {
                    case "mnuNewGame":
                        Game.Transit<GameScene>(Constants.NewGameFlag);
                        break;
                    case "mnuContinue":
                        Game.Transit<GameScene>(Constants.ContinueGameFlag);
                        break;
                    case "mnuControllerOptions":
                        Game.Transit<ControllerSettingScene>();
                        break;
                    case "mnuExit":
                        Game.Exit();
                        break;
                }
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _bgm?.Stop();

                    _bgmEffect?.Dispose();
                }

                base.Dispose(disposing);
                _disposed = true;
            }
        }
    }
}

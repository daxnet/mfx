using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FontStashSharp;
using Mfx.Core.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TetrisSharp.Input;

namespace TetrisSharp.Scenes
{
    internal sealed class ControllerSettingScene : Scene
    {
        private readonly FontSystem _fontSystem = new();
        private DynamicSpriteFont? _titleFont;
        private Vector2 _titleFontSize;
        private readonly Dictionary<GameKeys, string> _keyDict = new();

        public ControllerSettingScene(TetrisGame game, string name)
            : base(game, name, Color.FromNonPremultiplied(0, 130, 190, 255))
        {

        }

        public override void Load(ContentManager contentManager)
        {
            _fontSystem.AddFont(File.ReadAllBytes(@"res\main.ttf"));
            _titleFont = _fontSystem.GetFont(56);
            _titleFontSize = _titleFont.MeasureString("Controller Settings");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            spriteBatch.DrawString(_titleFont, "Controller Settings",
                new Vector2((Viewport.Width - _titleFontSize.X) / 2, 20), Color.White);
        }
    }
}

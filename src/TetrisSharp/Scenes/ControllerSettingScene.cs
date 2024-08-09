using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FontStashSharp;
using Mfx.Core.Elements;
using Mfx.Core.Elements.InputConfiguration;
using Mfx.Core.Scenes;
using Mfx.Extended.FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TetrisSharp.Scenes
{
    internal sealed class ControllerSettingScene : Scene
    {
        private readonly FontSystem _fontSystem = new();
        private DynamicSpriteFont? _titleFont;
        private DynamicSpriteFont? _inputConfigPanelFont;
        private Vector2 _titleFontSize;
        private InputConfigPanel? _inputConfigPanel;

        private readonly Dictionary<string, string> _settings = new Dictionary<string, string>()
        {
            { "Up", "" },
            { "Down", "" },
            { "Left", "" },
            { "Right", "" },
            { "Rotate", "" },
            { "Drop", "" },
            { "Pause", "" }
        };

        public ControllerSettingScene(TetrisGame game, string name)
            : base(game, name, Color.FromNonPremultiplied(0, 130, 190, 255))
        {

        }

        public override void Load(ContentManager contentManager)
        {
            _fontSystem.AddFont(File.ReadAllBytes(@"res\main.ttf"));
            _titleFont = _fontSystem.GetFont(56);
            _inputConfigPanelFont = _fontSystem.GetFont(30);
            _titleFontSize = _titleFont.MeasureString("Controller Settings");

            _inputConfigPanel = new InputConfigPanel(this, new FontStashSharpAdapter(_inputConfigPanelFont), _settings,
                240, 100, 300, Color.White, Color.Brown);

            Add(_inputConfigPanel);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            spriteBatch.DrawString(_titleFont, "Controller Settings",
                new Vector2((Viewport.Width - _titleFontSize.X) / 2, 20), Color.White);
        }
    }
}

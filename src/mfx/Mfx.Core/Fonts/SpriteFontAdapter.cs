using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mfx.Core.Fonts
{
    public sealed class SpriteFontAdapter : IFontAdapter
    {
        private readonly SpriteFont _spriteFont;

        public SpriteFontAdapter(SpriteFont spriteFont)
        {
            _spriteFont = spriteFont;
        }

        public Vector2 MeasureString(string text) => _spriteFont.MeasureString(text);

        public void DrawString(SpriteBatch spriteBatch, string text, Vector2 position, Color color)
            => spriteBatch.DrawString(_spriteFont, text, position, color);
    }
}

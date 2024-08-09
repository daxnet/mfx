using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mfx.Core.Fonts
{
    public interface IFontAdapter
    {
        Vector2 MeasureString(string text);

        void DrawString(SpriteBatch spriteBatch, string text, Vector2 position, Color color);
    }
}

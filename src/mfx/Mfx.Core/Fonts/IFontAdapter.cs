using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mfx.Core.Fonts
{
    /// <summary>
    /// Represents that the implemented classes are font adapters.
    /// </summary>
    public interface IFontAdapter
    {
        /// <summary>
        /// Draws a text string at the specified position with the specified color.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> used for drawing.</param>
        /// <param name="text">The text to be drawn</param>
        /// <param name="position">The position at where the text should be drawn.</param>
        /// <param name="color">The text drawing color.</param>
        void DrawString(SpriteBatch spriteBatch, string text, Vector2 position, Color color);

        /// <summary>
        /// Measures the size of a string text with the current font.
        /// </summary>
        /// <param name="text">The text to be measured.</param>
        /// <returns>A <see cref="Vector2"/> instance which represents the size of the text.</returns>
        Vector2 MeasureString(string text);
    }
}
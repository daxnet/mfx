// =============================================================================
//               __
//              / _|
//    _ __ ___ | |___  __
//   | '_ ` _ \|  _\ \/ /
//   | | | | | | |  >  <
//   |_| |_| |_|_| /_/\_\
//
// MIT License
//
// Copyright (c) 2024 Sunny Chen (daxnet)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// =============================================================================

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mfx.Core.Fonts;

/// <summary>
///     Represents that the implemented classes are font adapters.
/// </summary>
public interface IFontAdapter
{
    #region Public Methods

    /// <summary>
    ///     Draws a text string at the specified position with the specified color.
    /// </summary>
    /// <param name="spriteBatch">The <see cref="SpriteBatch" /> used for drawing.</param>
    /// <param name="text">The text to be drawn</param>
    /// <param name="position">The position at where the text should be drawn.</param>
    /// <param name="color">The text drawing color.</param>
    void DrawString(SpriteBatch spriteBatch, string text, Vector2 position, Color color);

    /// <summary>
    ///     Measures the size of a string text with the current font.
    /// </summary>
    /// <param name="text">The text to be measured.</param>
    /// <returns>A <see cref="Vector2" /> instance which represents the size of the text.</returns>
    Vector2 MeasureString(string text);

    #endregion Public Methods
}
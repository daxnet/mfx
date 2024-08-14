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

using Mfx.Core.Fonts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mfx.Core.Elements.Menus;

/// <summary>
///     Represents the based class of menu item effects.
/// </summary>
public abstract class MenuItemEffect
{
    #region Public Methods

    /// <summary>
    ///     Draws the menu item with the given parameters.
    /// </summary>
    /// <param name="hovering">
    ///     Indicates if the mouse cursor is currectly hovering over the menu item.
    /// </param>
    /// <param name="spriteBatch">
    ///     The <see cref="SpriteBatch" /> instance which is responsible for drawing the menu item.
    /// </param>
    /// <param name="menuItemFontAdapter">
    ///     The <see cref="IFontAdapter" /> that delegates the rendering of the menu item font to an underlying font object.
    /// </param>
    /// <param name="menuItem">The <see cref="MenuItem" /> to be drawn.</param>
    /// <param name="menuItemRect">
    ///     The <see cref="Rectangle" /> which represents the rectangle area where the menu item
    ///     could be drawn.
    /// </param>
    /// <param name="menuRect">
    ///     The <see cref="Rectangle" /> which represents the rectangle area of the entire menu.
    /// </param>
    public abstract void DrawMenuItem(bool hovering, SpriteBatch spriteBatch, IFontAdapter menuItemFontAdapter,
        MenuItem menuItem,
        Rectangle menuItemRect, Rectangle menuRect);

    #endregion Public Methods
}
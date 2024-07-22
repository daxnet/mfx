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

namespace Mfx.Core.Elements.Menus;

/// <summary>
///     Represents the menu item effect that renders menu items with simple colors.
/// </summary>
/// <param name="menuItemColor">The color of the menu item in a clickable state.</param>
/// <param name="hoverColor">The color of the menu item when mouse cursor hovers over it.</param>
/// <param name="disabledColor">The color of the menu item when it is disabled.</param>
public sealed class SimpleColorMenuItemEffect(Color menuItemColor, Color hoverColor, Color disabledColor)
    : MenuItemEffect
{
    #region Public Methods

    /// <inheritdoc />
    public override void DrawMenuItem(bool hovering, SpriteBatch spriteBatch, SpriteFont menuItemFont,
        MenuItem menuItem,
        Rectangle menuItemRect, Rectangle menuRect)
    {
        spriteBatch.DrawString(menuItemFont,
            menuItem.Text,
            new Vector2(menuItemRect.X, menuItemRect.Y),
            menuItem.Enabled ? hovering ? hoverColor : menuItemColor : disabledColor);
    }

    #endregion Public Methods
}
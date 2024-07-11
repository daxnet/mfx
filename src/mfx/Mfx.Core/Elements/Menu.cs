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

using Mfx.Core.Elements.Messages;
using Mfx.Core.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mfx.Core.Elements;

public class Menu : VisibleComponent
{

    #region Private Fields

    private readonly Color _hoverColor;
    private readonly Dictionary<string, Rectangle> _itemRegions = new();
    private readonly Color _menuColor;
    private readonly SpriteFont _spriteFont;
    private string _selectedMenuItem = string.Empty;

    #endregion Private Fields

    #region Public Constructors

    public Menu(IScene scene, SpriteFont spriteFont, string[] menuItems, float x, float y, Color menuColor,
        Color hoverColor, float linespace = 10,
        float margin = 5, Alignment alignment = Alignment.Center) : base(scene, spriteFont.Texture, x, y)
    {
        if (menuItems.Length == 0)
        {
            throw new ArgumentException("No menu item has been added to the menu.", nameof(menuItems));
        }

        _spriteFont = spriteFont;
        _menuColor = menuColor;
        _hoverColor = hoverColor;

        var boxWidth =
            menuItems.Select(i => _spriteFont.MeasureString(i).X)
                .Max() // Width of the menu block is determined by the max width of the menu
            + margin * 2; // Top and bottom margins

        var boxHeight =
            menuItems.Sum(i =>
                _spriteFont.MeasureString(i)
                    .Y) + // Height of the menu block is determined by the total height of all menu items
            margin * 2 + // Left and right margins
            linespace * (menuItems.Length - 1); // Adding up total linespaces

        var menuBox = new Rectangle((int)Math.Ceiling(x), (int)Math.Ceiling(y), (int)Math.Ceiling(boxWidth), (int)Math.Ceiling(boxHeight));

        var curItemIdx = 0;
        foreach (var menuItem in menuItems)
        {
            var size = _spriteFont.MeasureString(menuItem);
            var itemX = alignment switch
            {
                Alignment.Left => x + margin,
                Alignment.Center => x + margin + (menuBox.Width - size.X) / 2,
                Alignment.Right => x + menuBox.Width - margin - size.X,
                _ => 0
            };

            var itemY = y + margin + curItemIdx * (size.Y + linespace);

            _itemRegions.Add(menuItem,
                new Rectangle((int)Math.Ceiling(itemX), (int)Math.Ceiling(itemY), (int)Math.Ceiling(size.X),
                    (int)Math.Ceiling(size.Y)));

            curItemIdx++;
        }
    }

    #endregion Public Constructors

    #region Public Enums

    /// <summary>
    /// 
    /// </summary>
    public enum Alignment
    {
        Left,
        Center,
        Right,
    }

    #endregion Public Enums

    #region Public Methods

    public override void Update(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();
        _selectedMenuItem = string.Empty;
        foreach (var region in _itemRegions)
        {
            if (mouseState.X >= region.Value.X && mouseState.X <= region.Value.X + region.Value.Width &&
                mouseState.Y >= region.Value.Y && mouseState.Y <= region.Value.Y + region.Value.Height)
            {
                _selectedMenuItem = region.Key;
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    Publish(new MenuItemClickedMessage(_selectedMenuItem, mouseState.X, mouseState.Y));
                }
            }
        }

        base.Update(gameTime);
    }

    #endregion Public Methods

    #region Protected Methods

    protected override void ExecuteDraw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        foreach (var item in _itemRegions)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(_spriteFont, item.Key, new Vector2(item.Value.X, item.Value.Y),
                string.Equals(_selectedMenuItem, item.Key) ? _hoverColor : _menuColor);
            spriteBatch.End();
        }
    }

    #endregion Protected Methods
}
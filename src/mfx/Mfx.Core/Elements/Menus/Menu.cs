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
using Mfx.Core.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mfx.Core.Elements.Menus;

public class Menu : VisibleComponent
{
    #region Private Fields

    private readonly Dictionary<string, Rectangle> _itemRegions = new();
    private readonly Rectangle _menuBox;
    private readonly MenuItemEffect _menuItemEffect;
    private readonly MenuItem[] _menuItems;
    private readonly IFontAdapter _fontAdapter;
    private string _selectedMenuItemName = string.Empty;

    #endregion Private Fields

    #region Public Constructors

    public Menu(IScene scene, SpriteFont spriteFont, MenuItem[] menuItems, float x, float y, Color menuItemColor,
        Color hoverColor, Color disabledColor, float linespace = 10,
        float margin = 5, Alignment alignment = Alignment.Center)
        : this(scene, new SpriteFontAdapter(spriteFont), menuItems, x, y, menuItemColor, hoverColor, disabledColor,
            linespace, margin, alignment)
    {

    }

    public Menu(IScene scene, IFontAdapter fontAdapter, MenuItem[] menuItems, float x, float y, Color menuItemColor,
        Color hoverColor, Color disabledColor, float linespace = 10,
        float margin = 5, Alignment alignment = Alignment.Center)
        : this(scene, fontAdapter, menuItems, new SimpleColorMenuItemEffect(menuItemColor, hoverColor, disabledColor), x,
            y, linespace, margin, alignment)
    {
    }

    public Menu(IScene scene, SpriteFont spriteFont, MenuItem[] menuItems, MenuItemEffect menuItemEffect, float x,
        float y, float linespace = 10,
        float margin = 5, Alignment alignment = Alignment.Center)
        : this(scene, new SpriteFontAdapter(spriteFont), menuItems, menuItemEffect, x, y, linespace, margin, alignment)
    {

    }

    public Menu(IScene scene, IFontAdapter fontAdapter, MenuItem[] menuItems, MenuItemEffect menuItemEffect, float x,
        float y, float linespace = 10,
        float margin = 5, Alignment alignment = Alignment.Center) : base(scene, null, x, y)
    {
        if (menuItems.Length == 0)
        {
            throw new ArgumentException("No menu item has been added to the menu.", nameof(menuItems));
        }

        _fontAdapter = fontAdapter;
        _menuItems = menuItems;
        _menuItemEffect = menuItemEffect;

        var boxWidth =
            menuItems.Select(i => _fontAdapter.MeasureString(i.Text).X)
                .Max() // Width of the menu block is determined by the max width of the menu
            + margin * 2; // Top and bottom margins

        var boxHeight =
            menuItems.Sum(i =>
                _fontAdapter.MeasureString(i.Text)
                    .Y) + // Height of the menu block is determined by the total height of all menu items
            margin * 2 + // Left and right margins
            linespace * (menuItems.Length - 1); // Adding up total linespaces

        _menuBox = new Rectangle((int)Math.Ceiling(x), (int)Math.Ceiling(y), (int)Math.Ceiling(boxWidth),
            (int)Math.Ceiling(boxHeight));

        var curItemIdx = 0;
        foreach (var menuItem in menuItems)
        {
            var size = _fontAdapter.MeasureString(menuItem.Text);
            var itemX = alignment switch
            {
                Alignment.Left => x + margin,
                Alignment.Center => x + margin + (_menuBox.Width - size.X) / 2,
                Alignment.Right => x + _menuBox.Width - margin - size.X,
                _ => 0
            };

            var itemY = y + margin + curItemIdx * (size.Y + linespace);

            _itemRegions.Add(menuItem.Name,
                new Rectangle((int)Math.Ceiling(itemX), (int)Math.Ceiling(itemY), (int)Math.Ceiling(size.X),
                    (int)Math.Ceiling(size.Y)));

            curItemIdx++;
        }
    }

    #endregion Public Constructors

    #region Public Enums

    /// <summary>
    /// </summary>
    public enum Alignment
    {
        Left,
        Center,
        Right
    }

    #endregion Public Enums

    #region Public Methods

    /// <summary>
    ///     Retrieves the <see cref="MenuItem" /> with the specified name.
    /// </summary>
    /// <param name="name">The name of the menu item.</param>
    /// <returns>Menu item.</returns>
    public MenuItem? GetMenuItem(string name)
    {
        return _menuItems.FirstOrDefault(mi => mi.Name == name);
    }

    public override void Update(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();
        _selectedMenuItemName = string.Empty;
        foreach (var region in _itemRegions)
        {
            if (mouseState.X >= region.Value.X && mouseState.X <= region.Value.X + region.Value.Width &&
                mouseState.Y >= region.Value.Y && mouseState.Y <= region.Value.Y + region.Value.Height &&
                (_menuItems.FirstOrDefault(mi => mi.Name == region.Key)?.Enabled ?? false))
            {
                Mouse.SetCursor(MouseCursor.Hand);
                _selectedMenuItemName = region.Key;
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    Publish(new MenuItemClickedMessage(_selectedMenuItemName, mouseState.X, mouseState.Y));
                }
            }
        }

        if (string.IsNullOrEmpty(_selectedMenuItemName))
        {
            Mouse.SetCursor(MouseCursor.Arrow);
        }

        base.Update(gameTime);
    }

    #endregion Public Methods

    #region Protected Methods

    protected override void ExecuteDraw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        foreach (var item in _itemRegions)
        {
            var menuItem = _menuItems.FirstOrDefault(mi => mi.Name == item.Key);
            if (menuItem is null)
            {
                continue;
            }

            //spriteBatch.Begin();
            _menuItemEffect.DrawMenuItem(string.Equals(_selectedMenuItemName, item.Key),
                spriteBatch,
                _fontAdapter,
                menuItem,
                item.Value,
                _menuBox);
            //spriteBatch.End();
        }
    }

    #endregion Protected Methods
}
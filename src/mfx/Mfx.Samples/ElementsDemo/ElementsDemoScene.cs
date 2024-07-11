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

using Mfx.Core;
using Mfx.Core.Elements;
using Mfx.Core.Elements.Messages;
using Mfx.Core.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Mfx.Samples.ElementsDemo;

internal sealed class ElementsDemoScene(MfxGame game) : Scene(game, Color.CornflowerBlue)
{
    #region Private Fields

    private SpriteFont? _labelFont;
    private Menu? _menu;
    private SpriteFont? _menuFont;
    private StaticLabel? _selectedMenuTextLabel;

    #endregion Private Fields

    #region Public Methods

    public override void Load(ContentManager contentManager)
    {
        _labelFont = contentManager.Load<SpriteFont>("arial");
        _menuFont = contentManager.Load<SpriteFont>("times_menu");

        _menu = new Menu(this, _menuFont, ["New Game", "Options", "Load From Existing Saving", "Exit"], 100, 200,
            Color.White, Color.Red);

        _selectedMenuTextLabel = new StaticLabel("", this, _labelFont, 0, 25, Color.Yellow);

        Add(new StaticLabel("This is a static label.", this, _labelFont, 0, 0, Color.Yellow));
        Add(_selectedMenuTextLabel);
        Add(_menu);

        Subscribe<MenuItemClickedMessage>((publisher, message) =>
        {
            _selectedMenuTextLabel.Text = $"Selected Menu Item: {message.MenuItem}";
        });
    }

    #endregion Public Methods
}
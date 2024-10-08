﻿// =============================================================================
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

using System.Collections.Generic;
using Mfx.Core;
using Mfx.Core.Elements;
using Mfx.Core.Elements.InputConfiguration;
using Mfx.Core.Elements.Menus;
using Mfx.Core.Fonts;
using Mfx.Core.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Mfx.Samples.ElementsDemo;

internal sealed class ElementsDemoScene(MfxGame game, string name) : Scene(game, name, Color.Black)
{
    #region Private Fields

    private SpriteFont? _labelFont;
    private Menu? _menu;
    private SpriteFont? _menuFont;
    private Label? _selectedMenuTextLabel;
    private InputConfigPanel? _inputConfigPnl;

    #endregion Private Fields

    #region Public Methods

    public override void Load(ContentManager contentManager)
    {
        _labelFont = contentManager.Load<SpriteFont>("arial");
        _menuFont = contentManager.Load<SpriteFont>("times_menu");

        _menu = new Menu(this, _menuFont, [
            new MenuItem("mnuNewGame", "New Game"),
            new MenuItem("mnuOptions", "Options"),
            new MenuItem("mnuResetKeys", "Reset Keys"),
            new MenuItem("mnuLoadExistingSaving", "Load From Existing Saving") { Enabled = false },
            new MenuItem("mnuExit", "Exit")
        ], 100, 200, Color.White, Color.Red, Color.Gray);

        _selectedMenuTextLabel = new Label("", this, _labelFont, 0, 25, Color.Yellow);
        _inputConfigPnl = new InputConfigPanel(this, new SpriteFontAdapter(_menuFont),
            ["Up", "Down", "Left", "Right"], 500, 200, 270, Color.YellowGreen, Color.Brown);

        Add(new Label("This is a static label.", this, _labelFont, 0, 0, Color.Yellow));
        Add(_selectedMenuTextLabel);
        Add(_menu);
        Add(_inputConfigPnl);

        Subscribe<MenuItemClickedMessage>((_, message) =>
        {
            if (message.MenuItemName == "mnuResetKeys")
            {
                _inputConfigPnl.Reset();
                return;
            }
            // _selectedMenuTextLabel.Text = $"Selected Menu Item: {message.MenuItem}";
            var nextSceneName = message.MenuItemName switch
            {
                "mnuNewGame" => "NewGameScene",
                "mnuExit" => "Exit",
                _ => null
            };

            if (nextSceneName is null)
            {
                _selectedMenuTextLabel.Text = $"Selected Menu Item: {message.MenuItemName}";
            }
            else if (nextSceneName == "Exit")
            {
                Game.Exit();
            }
            else
            {
                Game.Transit(nextSceneName);
            }
        });
    }

    #endregion Public Methods
}
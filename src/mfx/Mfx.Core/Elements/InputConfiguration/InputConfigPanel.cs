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
using Mfx.Core.Input;
using Mfx.Core.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mfx.Core.Elements.InputConfiguration;

public class InputConfigPanel : VisibleComponent
{
    #region Private Fields

    private static readonly TimeSpan _interval = TimeSpan.FromMilliseconds(150);
    private readonly Color _color;
    private readonly Color _currentItemColor;
    private readonly IFontAdapter _fontAdapter;
    private readonly float _itemHeight;
    private readonly Dictionary<string, string> _keyMappings;
    private readonly float _width;
    private int _currentIndex;
    private TimeSpan _ticks;

    #endregion Private Fields

    #region Public Constructors

    public InputConfigPanel(IScene scene, IFontAdapter fontAdapter,
        IEnumerable<KeyValuePair<string, string>> keyMappings, float x,
        float y, float width)
        : this(scene, fontAdapter, keyMappings, x, y, width, Color.White, Color.Red)
    {
    }

    public InputConfigPanel(IScene scene, IFontAdapter fontAdapter,
        IEnumerable<KeyValuePair<string, string>> keyMappings, float x,
        float y, float width, Color color, Color currentItemColor) : base(scene, null, x, y)
    {
        _keyMappings = keyMappings.ToDictionary();
        _width = width;
        _fontAdapter = fontAdapter;
        _color = color;
        _currentItemColor = currentItemColor;

        _itemHeight = fontAdapter.MeasureString(_keyMappings.Keys.First()).Y;
    }

    #endregion Public Constructors

    #region Public Properties

    public IEnumerable<KeyValuePair<string, string>> KeyMappings => _keyMappings;

    #endregion Public Properties

    #region Public Methods

    public void Reset()
    {
        _currentIndex = 0;
        foreach (var kvp in _keyMappings)
        {
            _keyMappings[kvp.Key] = "(none)";
        }
    }

    public override void Update(GameTime gameTime)
    {
        if (_currentIndex == _keyMappings.Count)
        {
            return;
        }

        _ticks += gameTime.ElapsedGameTime;
        if (_ticks > _interval)
        {
            var pressedKeys = VirtualInput.GetPressedVirtualKeys();
            if (pressedKeys.Length > 0)
            {
                var curKey = _keyMappings.ElementAt(_currentIndex).Key;
                _keyMappings[curKey] = pressedKeys[0];
                _currentIndex++;
            }

            _ticks = TimeSpan.Zero;
        }
    }

    #endregion Public Methods

    #region Protected Methods

    protected override void ExecuteDraw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        for (var idx = 0; idx < _keyMappings.Count; idx++)
        {
            var kvp = _keyMappings.ElementAt(idx);
            var color = idx == _currentIndex ? _currentItemColor : _color;
            // Draw key
            spriteBatch.DrawString(_fontAdapter, kvp.Key, new Vector2(X, Y + idx * _itemHeight), color);

            // Draw value
            var kValue = string.IsNullOrEmpty(kvp.Value) ? "(none)" : kvp.Value;
            var currentValueTextSize = _fontAdapter.MeasureString(kValue);
            var posX = X + _width - currentValueTextSize.X;
            spriteBatch.DrawString(_fontAdapter, kValue, new Vector2(posX, Y + idx * _itemHeight), color);
        }
    }

    #endregion Protected Methods
}
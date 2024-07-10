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

using System;
using Mfx.Core;
using Mfx.Core.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mfx.Samples.FlyingObject;

internal sealed class FlyingObjectScene(MfxGame game) : Scene(game, Color.Black)
{
    #region Protected Methods

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _spriteTexture?.Dispose();
            }

            base.Dispose(disposing);
            _disposed = true;
        }
    }

    #endregion Protected Methods

    #region Private Fields

    private const int NumberOfSprites = 3;

    // ReSharper disable once InconsistentNaming
    private static readonly Random _rnd = new(DateTime.UtcNow.Millisecond);

    private bool _disposed;

    private Texture2D? _spriteTexture;

    #endregion Private Fields

    #region Public Methods

    public override void Load(ContentManager contentManager)
    {
        var screenWidth = Game.GraphicsDevice.Viewport.Width;
        var screenHeight = Game.GraphicsDevice.Viewport.Height;
        _spriteTexture = contentManager.Load<Texture2D>("dog");

        for (var i = 0; i < NumberOfSprites; i++)
        {
            var initialX = _rnd.Next(1, screenWidth - _spriteTexture.Width);
            var initialY = _rnd.Next(1, screenHeight - _spriteTexture.Height);
            var initialDeltaX = _rnd.Next(5) + 1;
            var initialDeltaY = _rnd.Next(5) + 1;

            var objectSprite = new FlyingObjectSprite(this, _spriteTexture, initialX, initialY, initialDeltaX,
                initialDeltaY);

            Add(objectSprite);
        }
    }

    public override void Update(GameTime gameTime)
    {
        if (!Ended && Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            End();
        }

        base.Update(gameTime);
    }

    #endregion Public Methods
}
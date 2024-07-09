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
using System.Linq;
using Mfx.Core;
using Mfx.Core.Physics;
using Mfx.Core.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mfx.Samples.Particles;

internal sealed class ParticleScene : Scene
{
    #region Private Fields

    private const int CenterAreaHeight = 16;
    private const int CenterAreaWidth = 20;

    // ReSharper disable once InconsistentNaming
    private static readonly Random _rnd = new(DateTime.UtcNow.Millisecond);

    private readonly TimeSpan _generateStarInterval = TimeSpan.FromMilliseconds(5);
    private TimeSpan _interval = TimeSpan.Zero;

    #endregion Private Fields

    #region Public Constructors

    public ParticleScene(MfxGame game)
        : base(game, Color.Black)
    {
        Subscribe<OutOfViewportMessage>((publisher, _) =>
        {
            if (publisher is ParticleSprite sprite) sprite.Texture?.Dispose();
        });
    }

    #endregion Public Constructors

    #region Public Methods

    public override void Load(ContentManager contentManager)
    {
    }

    public override void Update(GameTime gameTime)
    {
        if (!Ended && Keyboard.GetState().IsKeyDown(Keys.Escape)) End();

        _interval += gameTime.ElapsedGameTime;
        if (_interval > _generateStarInterval)
        {
            var x = (Viewport.Width - CenterAreaWidth) / 2.0f + _rnd.Next(CenterAreaWidth);
            var y = (Viewport.Height - CenterAreaHeight) / 2.0f + _rnd.Next(CenterAreaHeight);
            var texture = new Texture2D(Game.GraphicsDevice, 4, 4);
            var color = new Color(_rnd.Next(256), _rnd.Next(256), _rnd.Next(256));
            texture.SetData(Enumerable.Repeat(color, 16).ToArray());
            var sprite = new ParticleSprite(this, texture, x, y);
            Add(sprite);
            _interval = TimeSpan.Zero;
        }

        base.Update(gameTime);
    }

    #endregion Public Methods
}
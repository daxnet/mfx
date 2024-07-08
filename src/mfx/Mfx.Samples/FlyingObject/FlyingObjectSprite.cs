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
using Mfx.Core.Messaging;
using Mfx.Core.Scenes;
using Mfx.Core.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mfx.Samples.FlyingObject;

internal sealed class FlyingObjectSprite : Sprite
{
    #region Public Constructors

    public FlyingObjectSprite(IScene scene, Texture2D texture, float x, float y, int dx, int dy)
        : base(scene, texture, x, y)
    {
        _dx = dx;
        _dy = dy;
        Subscribe<BoundaryHitMessage>((p, m) =>
        {
            if (Equals(p))
            {
                if ((m.Boundary & Boundary.Left) == Boundary.Left ||
                    (m.Boundary & Boundary.Right) == Boundary.Right)
                    _dx = -Math.Sign(_dx) * _deltaOptions[_rnd.Next(_deltaOptions.Length)];

                if ((m.Boundary & Boundary.Top) == Boundary.Top ||
                    (m.Boundary & Boundary.Bottom) == Boundary.Bottom)
                    _dy = -Math.Sign(_dy) * _deltaOptions[_rnd.Next(_deltaOptions.Length)];
            }
        });
        EnableBoundaryDetection = true;
    }

    #endregion Public Constructors

    #region Public Methods

    public override void Update(GameTime gameTime)
    {
        X += _dx;
        Y += _dy;

        var screenWidth = Scene.Game.GraphicsDevice.Viewport.Width;
        var screenHeight = Scene.Game.GraphicsDevice.Viewport.Height;

        if (X < 0)
            X = 0;
        if (Y < 0)
            Y = 0;

        if (X > screenWidth - Width)
            X = screenWidth - Width;

        if (Y > screenHeight - Height)
            Y = screenHeight - Height;

        base.Update(gameTime);
    }

    #endregion Public Methods

    #region Private Fields

    private static readonly float[] _deltaOptions =
    [
        1.0f,
        1.5f,
        2.0f,
        2.5f,
        3.0f,
        3.5f,
        4.0f,
        5.0f,
        8.0f,
        10.0f,
        15.0f
    ];

    // ReSharper disable once InconsistentNaming
    private static readonly Random _rnd = new(DateTime.UtcNow.Millisecond);

    private float _dx;
    private float _dy;

    #endregion Private Fields
}
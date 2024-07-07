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

using Mfx.Core.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mfx.Core;

/// <summary>
///     Represents that the implemented classes are visible components that can be placed and viewed
///     on the game surface.
/// </summary>
public interface IVisibleComponent : IComponent, IMessagePublisher, IMessageSubscriber
{
    #region Public Methods

    void Draw(GameTime gameTime, SpriteBatch spriteBatch);

    #endregion Public Methods

    #region Public Properties

    Rectangle? BoundingBox { get; }

    bool Collidable { get; set; }

    /// <summary>
    ///     Gets or sets a <see cref="bool" /> value which indicates if the boundary
    ///     detection should be performed while the current visible component is moving
    ///     on the scene.
    /// </summary>
    /// <remarks>
    ///     If this property is set to <c>true</c>, a <see cref="BoundaryHitMessage" /> message will
    ///     be dispatched to the system when the current visible component hits the boundary of the <see cref="Viewport" />.
    /// </remarks>
    bool EnableBoundaryDetection { get; set; }

    int Layer { get; set; }
    Texture2D? Texture { get; }
    bool Visible { get; set; }
    float X { get; set; }

    float Y { get; set; }

    #endregion Public Properties
}
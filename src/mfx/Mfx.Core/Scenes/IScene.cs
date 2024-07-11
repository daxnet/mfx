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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Mfx.Core.Scenes;

/// <summary>
///     Represents a 2D scene in the game.
/// </summary>
public interface IScene : ICollection<IComponent>, IComponent, IDrawable, IMessagePublisher, IMessageSubscriber,
    IDisposable
{

    #region Public Properties

    /// <summary>
    ///     Gets the background color.
    /// </summary>
    Color BackgroundColor { get; }

    /// <summary>
    ///     Gets the instance of the <see cref="MfxGame" /> to which the current scene belongs.
    /// </summary>
    MfxGame Game { get; }

    /// <summary>
    /// Gets the name of the scene.
    /// </summary>
    string Name { get; }
    /// <summary>
    ///     Gets or sets the next scene, set it to <c>null</c> if the current one is the last scene.
    /// </summary>
    /// <remarks>
    ///     <see cref="MfxGame" /> will end once it finishes performing the current scene and the
    ///     <c>Next</c> property of the current scene is <c>null</c>.
    /// </remarks>
    IScene? Next { get; set; }

    /// <summary>
    ///     Gets the <see cref="Viewport" /> of the scene.
    /// </summary>
    Viewport Viewport { get; }

    #endregion Public Properties

    #region Public Methods

    /// <summary>
    ///     A callback method that is executed each time when the current scene is going to be the active scene of the game.
    /// </summary>
    void Enter();

    /// <summary>
    ///     A callback method that is executed each time when the current scene is going to be an inactive scene of the game.
    /// </summary>
    void Leave();

    /// <summary>
    ///     Loads the required contents for the current scene to run.
    /// </summary>
    /// <param name="contentManager">The <see cref="ContentManager" /> from where the contents or resources are loaded.</param>
    void Load(ContentManager contentManager);

    #endregion Public Methods

}
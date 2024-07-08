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
using Mfx.Core.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mfx.Core;

public abstract class VisibleComponent(IScene scene, Texture2D? texture, float x, float y)
    : Component, IVisibleComponent
{
    #region Protected Properties

    protected IScene Scene { get; } = scene;

    #endregion Protected Properties

    #region Protected Methods

    protected abstract void ExecuteDraw(GameTime gameTime, SpriteBatch spriteBatch);

    #endregion Protected Methods

    #region Protected Constructors

    protected VisibleComponent(IScene scene)
        : this(scene, null)
    {
    }

    protected VisibleComponent(IScene scene, Texture2D? texture)
        : this(scene, texture, 0, 0)
    {
    }

    #endregion Protected Constructors

    #region Public Properties

    public Rectangle? BoundingBox
    {
        get
        {
            if (Width == 0 || Height == 0) return null;
            return new Rectangle((int)Math.Ceiling(X), (int)Math.Ceiling(Y), Width, Height);
        }
    }

    public bool Collidable { get; set; } = true;
    public bool EnableBoundaryDetection { get; set; } = false;
    public virtual int Height => Texture?.Height ?? 0;
    public int Layer { get; set; } = 0;
    public Texture2D? Texture { get; } = texture;
    public bool Visible { get; set; } = true;
    public virtual int Width => Texture?.Width ?? 0;
    public virtual float X { get; set; } = x;
    public virtual float Y { get; set; } = y;

    #endregion Public Properties

    #region Public Methods

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (Visible) ExecuteDraw(gameTime, spriteBatch);
    }

    public void Publish<TMessage>(TMessage message) where TMessage : IMessage
    {
        Scene.Game.MessageDispatcher.Dispatch(this, message);
    }

    public void Subscribe<TMessage>(Action<object, TMessage> handler) where TMessage : IMessage
    {
        Scene.Game.MessageDispatcher.RegisterHandler(handler);
    }

    public override string ToString()
    {
        return Id.ToString();
    }

    public override void Update(GameTime gameTime)
    {
        if (!EnableBoundaryDetection)
            return;

        var viewport = Scene.Game.GraphicsDevice.Viewport;
        var result = Boundary.None;
        if (X <= 0)
            result |= Boundary.Left;
        if (Y <= 0)
            result |= Boundary.Top;
        if (X >= viewport.Width - Width)
            result |= Boundary.Right;
        if (Y >= viewport.Height - Height)
            result |= Boundary.Bottom;

        if (result != Boundary.None)
        {
            Publish(new BoundaryHitMessage(result));
        }
    }

    #endregion Public Methods
}
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

using System.Collections;
using System.Collections.Concurrent;
using Mfx.Core.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Mfx.Core.Scenes;

public abstract class Scene(MfxGame game, Texture2D? texture, Color backgroundColor) : IScene
{

    #region Private Fields

    private readonly ConcurrentDictionary<Guid, IComponent> _components = new();

    #endregion Private Fields

    #region Protected Constructors

    protected Scene(MfxGame game)
        : this(game, null, Color.CornflowerBlue)
    {
    }

    protected Scene(MfxGame game, Color backgroundColor)
        : this(game, null, backgroundColor)
    {
    }

    #endregion Protected Constructors

    #region Public Properties

    public bool AutoRemoveInactiveComponents { get; protected set; } = true;

    public Color BackgroundColor { get; } = backgroundColor;

    public Rectangle? BoundingBox
    {
        get
        {
            if (Width == 0 || Height == 0) return null;
            return new Rectangle((int)Math.Ceiling(X), (int)Math.Ceiling(Y), Width, Height);
        }
    }

    public bool Collidable { get; set; }

    public int Count => _components.Count;

    public bool EnableBoundaryDetection { get; set; }

    public MfxGame Game { get; } = game;

    public int Height => Texture?.Height ?? 0;

    public Guid Id { get; } = Guid.NewGuid();

    public bool IsActive { get; set; }

    public bool IsReadOnly => false;

    public int Layer { get; set; }

    public IScene? Next { get; set; }

    public Texture2D? Texture { get; } = texture;

    public bool Visible { get; set; }

    public int Width => Texture?.Width ?? 0;

    public float X { get; set; } = 0;

    public float Y { get; set; } = 0;

    #endregion Public Properties

    #region Public Methods

    public void Add(IComponent item) => _components.TryAdd(item.Id, item);

    public void Clear() => _components.Clear();

    public bool Contains(IComponent item)
    {
        return _components.ContainsKey(item.Id);
    }

    public void CopyTo(IComponent[] array, int arrayIndex)
    {
        _components.Values.CopyTo(array, arrayIndex);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        Game.GraphicsDevice.Clear(BackgroundColor);
        _components
            .Values
            .Where(v => v is IVisibleComponent)
            .Cast<IVisibleComponent>()
            .OrderBy(v => v.Layer)
            .ToList()
            .ForEach(v => v.Draw(gameTime, spriteBatch));
    }

    public virtual void Enter()
    {
    }

    public bool Equals(IComponent? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return other is IScene scene && Id.Equals(scene.Id);
    }

    public IEnumerator<IComponent> GetEnumerator() => _components.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _components.Values.GetEnumerator();

    public virtual void Leave()
    {
    }

    public abstract void Load(ContentManager contentManager);

    public void Publish<TMessage>(TMessage message) where TMessage : IMessage => Game.MessageDispatcher.Dispatch(this, message);

    public bool Remove(IComponent item) => _components.TryRemove(item.Id, out _);

    public void Subscribe<TMessage>(Action<object, TMessage> handler) where TMessage : IMessage => Game.MessageDispatcher.RegisterHandler(handler);

    public void Update(GameTime gameTime)
    {
        _components
            .Values
            .Where(component => component.IsActive)
            .AsParallel()
            .ForAll(component => component.Update(gameTime));
    }

    #endregion Public Methods

    #region Protected Methods

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            // TODO release managed resources here
        }
    }

    #endregion Protected Methods
}
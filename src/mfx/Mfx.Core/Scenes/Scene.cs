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
using System.ComponentModel;
using Mfx.Core.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Mfx.Core.Scenes;

public abstract class Scene(MfxGame game, string name, Color backgroundColor) : IScene
{

    #region Private Fields

    private readonly ConcurrentDictionary<Guid, IComponent> _components = new();

    #endregion Private Fields

    #region Protected Constructors

    protected Scene(MfxGame game, string name)
        : this(game, name, Color.CornflowerBlue)
    {
    }

    #endregion Protected Constructors

    #region Public Properties

    public Color BackgroundColor { get; } = backgroundColor;
    public int Count => _components.Count;
    public MfxGame Game { get; } = game;
    public Guid Id { get; } = Guid.NewGuid();
    public bool IsActive { get; set; }
    public bool IsReadOnly => false;
    public string Name { get; } = name;

    public bool Paused { get; private set; }
    public Viewport Viewport => Game.GraphicsDevice.Viewport;

    #endregion Public Properties

    #region Public Methods

    public void Add(IComponent item)
    {
        if (_components.TryAdd(item.Id, item) &&
            item is IVisibleComponent visibleComponent)
        {
            visibleComponent.OnAddedToScene(this);
        }
    }

    public void Clear()
    {
        Parallel.ForEach(_components.Values, component =>
        {
            if (component is IVisibleComponent visibleComponent)
            {
                visibleComponent.OnRemovedFromScene(this);
            }
        });

        _components.Clear();
    }

    public bool Contains(IComponent item)
    {
        return _components.ContainsKey(item.Id);
    }

    public void CopyTo(IComponent[] array, int arrayIndex) => _components.Values.CopyTo(array, arrayIndex);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (!Paused)
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
    }

    public virtual void Enter(object? args = null)
    {
    }

    public bool Equals(IComponent? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return other is IScene scene && Id.Equals(scene.Id);
    }

    public IEnumerator<IComponent> GetEnumerator() => _components.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _components.Values.GetEnumerator();

    public virtual void Leave()
    {
    }

    public abstract void Load(ContentManager contentManager);

    public void Pause()
    {
        Paused = true;
        OnPaused();
    }

    public void Publish<TMessage>(TMessage message) where TMessage : IMessage =>
            Game.MessageDispatcher.Dispatch(this, message);

    public bool Remove(IComponent item)
    {
        var result = _components.TryRemove(item.Id, out var removedComponent);
        if (removedComponent is IVisibleComponent visibleComponent)
        {
            visibleComponent.OnRemovedFromScene(this);
        }

        if (removedComponent is IDisposable disposable)
        {
            disposable.Dispose();
        }

        return result;
    }

    public void Resume()
    {
        Paused = false;
        OnResumed();
    }

    public void Subscribe<TMessage>(Action<object, TMessage> handler) where TMessage : IMessage =>
            Game.MessageDispatcher.RegisterHandler(handler);

    public virtual void Update(GameTime gameTime)
    {
        if (!Paused)
        {
            _components
                .Values
                .Where(component => component.IsActive)
                .AsParallel()
                .ForAll(component => component.Update(gameTime));

            var inactiveComponents = _components.Values.Where(component => !component.IsActive);
            Parallel.ForEach(inactiveComponents, component =>
            {
                Remove(component);
            });
        }
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

    protected virtual void OnPaused()
    {
    }

    protected virtual void OnResumed()
    {
    }

    #endregion Protected Methods

}
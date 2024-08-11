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

public class MfxGame : Game
{
    #region Protected Fields

    protected SpriteBatch? _spriteBatch;

    #endregion Protected Fields

    #region Private Fields

    private readonly GraphicsDeviceManager _graphicsDeviceManager;
    private readonly Dictionary<string, IScene> _scenes = new();
    private readonly MfxGameWindowOptions _settings;
    private IScene? _activeScene;
    private bool _disposed;

    #endregion Private Fields

    #region Public Constructors

    public MfxGame()
        : this(MfxGameWindowOptions.NormalScreenShowMouse)
    {
    }

    public MfxGame(MfxGameWindowOptions settings)
    {
        _graphicsDeviceManager = new GraphicsDeviceManager(this);
        _settings = settings;
        Content.RootDirectory = "Content";
    }

    #endregion Public Constructors

    #region Internal Properties

    internal MessageDispatcher MessageDispatcher { get; } = new();

    #endregion Internal Properties

    #region Public Methods

    /// <summary>
    ///     Transits to a scene with the specified name.
    /// </summary>
    /// <param name="sceneName">The name of the scene to be transited to.</param>
    public void Transit(string sceneName, object? args = null)
    {
        // Gets the scene by name.
        if (!_scenes.TryGetValue(sceneName, out var target))
        {
            throw new MfxException($"The scene '{sceneName}' doesn't exist.");
        }

        _activeScene?.Pause();
        _activeScene?.Leave();

        target.Enter(args);
        if (target.Paused)
        {
            target.Resume();
        }

        _activeScene = target;
    }

    public void Transit<TScene>(object? args = null)
        where TScene : class, IScene =>
        Transit(typeof(TScene).Name, args);

    #endregion Public Methods

    #region Protected Methods

    protected TScene AddScene<TScene>(string name)
        where TScene : class, IScene
    {
        var constructors = from p in typeof(TScene).GetConstructors()
            let parameters = p.GetParameters()
            where parameters.Length == 2 &&
                  (parameters[0].ParameterType == typeof(MfxGame) ||
                   parameters[0].ParameterType.IsSubclassOf(typeof(MfxGame))) &&
                  parameters[1].ParameterType == typeof(string)
            select p;
        if (!constructors.Any())
        {
            throw new MfxException($"No suitable constructor found on type {typeof(TScene).FullName}.");
        }

        if (Activator.CreateInstance(typeof(TScene), [this, name]) is not TScene scene)
        {
            throw new MfxException($"Unable to initialize a new instance of type {typeof(TScene).FullName}.");
        }

        _scenes.Add(name, scene);

        return scene;
    }

    protected TScene AddScene<TScene>() where TScene : class, IScene => AddScene<TScene>(typeof(TScene).Name);

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            // Triggers a leave operation on the current active scene.
            _activeScene?.Leave(true);

            if (disposing)
            {
                Parallel.ForEach(_scenes, s => s.Value.Dispose());
            }

            base.Dispose(disposing);
            _disposed = true;
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        if (_spriteBatch is not null && _activeScene is not null)
        {
            //_spriteBatch.Begin(SpriteBatchDrawOptions.SpriteSortMode,
            //    SpriteBatchDrawOptions.BlendState,
            //    SpriteBatchDrawOptions.SamplerState,
            //    SpriteBatchDrawOptions.DepthStencilState,
            //    SpriteBatchDrawOptions.RasterizerState,
            //    SpriteBatchDrawOptions.Effect,
            //    SpriteBatchDrawOptions.TransformMatrix);
            _spriteBatch.Begin();
            _activeScene.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();
        }

        base.Draw(gameTime);
    }

    protected override void Initialize()
    {
        if (_activeScene is null)
        {
            throw new MfxException("The current active scene is not specified.");
        }

        if (!string.IsNullOrEmpty(_settings.Title))
        {
            Window.Title = _settings.Title;
        }

        _graphicsDeviceManager.IsFullScreen = _settings.IsFullScreen;
        if (!_settings.IsFullScreen)
        {
            _graphicsDeviceManager.PreferredBackBufferWidth = _settings.Width;
            _graphicsDeviceManager.PreferredBackBufferHeight = _settings.Height;
        }
        else
        {
            _graphicsDeviceManager.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            _graphicsDeviceManager.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
        }

        Window.AllowUserResizing = _settings.AllowResizing;
        IsMouseVisible = _settings.MouseVisible;

        _graphicsDeviceManager.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        base.LoadContent();
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        foreach (var scene in _scenes) scene.Value.Load(Content);

        _activeScene?.Enter();
    }

    protected void StartFrom(string sceneName)
    {
        if (!_scenes.TryGetValue(sceneName, out var scene))
        {
            throw new MfxException($"The scene '{sceneName}' doesn't exist.");
        }

        _activeScene = scene;
    }

    protected void StartFrom<TScene>() where TScene : class, IScene => StartFrom(typeof(TScene).Name);

    protected override void Update(GameTime gameTime)
    {
        _activeScene?.Update(gameTime);
        base.Update(gameTime);
    }

    #endregion Protected Methods
}
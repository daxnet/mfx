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

using System.Reflection;
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
    private readonly MfxGameSettings _settings;
    private bool _disposed;

    #endregion Private Fields

    #region Public Constructors

    public MfxGame()
        : this(MfxGameSettings.NormalScreenShowMouse)
    {
    }

    public MfxGame(MfxGameSettings settings)
    {
        _graphicsDeviceManager = new GraphicsDeviceManager(this);
        _settings = settings;
        Content.RootDirectory = "Content";
        MessageDispatcher.RegisterHandler<SceneEndedMessage>((_, _) =>
        {
            ActiveScene?.Leave();
            ActiveScene = ActiveScene?.Next;
            if (ActiveScene is null)
            {
                Exit();
                return;
            }

            ActiveScene?.Enter();
        });
    }

    #endregion Public Constructors

    #region Internal Properties

    internal MessageDispatcher MessageDispatcher { get; } = new();

    #endregion Internal Properties

    #region Protected Properties

    protected IScene? FirstScene { get; set; }
    protected SpriteBatchDrawOptions SpriteBatchDrawOptions { get; set; } = SpriteBatchDrawOptions.Default;

    #endregion Protected Properties

    #region Private Properties

    private IScene? ActiveScene { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected TScene AddScene<TScene>(TScene? previousScene = null)
        where TScene : class, IScene
    {
        var constructors = from p in typeof(TScene).GetConstructors()
                           let parameters = p.GetParameters()
                           where parameters.Length == 1 &&
                                 parameters[0].ParameterType == typeof(MfxGame)
                           select p;
        if (!constructors.Any())
            throw new MfxException($"No suitable constructor found on type {typeof(TScene).FullName}.");

        if (Activator.CreateInstance(typeof(TScene), this) is not TScene scene)
            throw new MfxException($"Unable to initialize a new instance of type {typeof(TScene).FullName}.");

        if (previousScene is null)
            ActiveScene = scene;
        else
            previousScene.Next = scene;

        return scene;
    }

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing && FirstScene is not null)
            {
                var allScenes = new List<IScene>();
                var cur = FirstScene;
                while (cur.Next is not null)
                {
                    allScenes.Add(cur);
                    cur = cur.Next;
                }

                allScenes.ForEach(s => s.Dispose());
            }

            base.Dispose(disposing);
            _disposed = true;
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        if (_spriteBatch is not null && ActiveScene is not null)
        {
            _spriteBatch.Begin(SpriteBatchDrawOptions.SpriteSortMode,
                SpriteBatchDrawOptions.BlendState,
                SpriteBatchDrawOptions.SamplerState,
                SpriteBatchDrawOptions.DepthStencilState,
                SpriteBatchDrawOptions.RasterizerState,
                SpriteBatchDrawOptions.Effect,
                SpriteBatchDrawOptions.TransformMatrix);
            ActiveScene.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();
        }

        base.Draw(gameTime);
    }

    protected override void Initialize()
    {
        ActiveScene = FirstScene ?? throw new MfxException("FirstScene is not specified.");

        if (!string.IsNullOrEmpty(_settings.Title)) Window.Title = _settings.Title;

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
        if (FirstScene is null)
            return;

        var cur = FirstScene;
        while (cur.Next is not null)
        {
            cur.Load(Content);
            cur = cur.Next;
        }

        ActiveScene?.Enter();
    }

    protected override void Update(GameTime gameTime)
    {
        ActiveScene?.Update(gameTime);
        base.Update(gameTime);
    }

    #endregion Protected Methods

}
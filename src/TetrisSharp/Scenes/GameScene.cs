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
using System.Collections.Generic;
using Mfx.Core.Input;
using Mfx.Core.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TetrisSharp.Blocks;
using TetrisSharp.Messages;

namespace TetrisSharp.Scenes;

internal sealed class GameScene : Scene, IGameScene
{
    #region Private Fields

    private const float KeyDelay = 0.09f;
    private static readonly Random _rnd = new(DateTime.Now.Millisecond);

    private readonly BlockGenerator _blockGenerator = new("blocks.xml");
    private readonly Queue<int> _tetrisQueue = new();
    private readonly Texture2D[] _tileTextures = new Texture2D[Constants.TileTextureCount];
    private Block? _block;
    private int _boardX;
    private int _boardY;
    private bool _disposed;
    private Texture2D? _fixedTileTexture;
    private Texture2D? _gameboardTexture;
    private Block? _nextBlock;
    private float _timeSinceLastKeyPress;

    #endregion Private Fields

    #region Public Constructors

    public GameScene(TetrisGame game, string name)
        : base(game, name, Color.FromNonPremultiplied(0, 130, 190, 255))
    {
    }

    #endregion Public Constructors

    #region Public Properties

    public GameBoard? GameBoard { get; private set; }

    #endregion Public Properties

    #region Public Methods

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_gameboardTexture, new Vector2(_boardX, _boardY), Color.White);
        base.Draw(gameTime, spriteBatch);
    }

    public override void Enter(object? args = null)
    {
        if (Game is TetrisGame tg)
        {
            tg.CanContinue = true;
        }

        if (args is not string sArgs)
        {
            return;
        }

        switch (sArgs)
        {
            case Constants.NewGameFlag:
                ResetGame();
                break;
        }
    }

    public override void Load(ContentManager contentManager)
    {
        _boardX = 30;
        _boardY = (Viewport.Height - 25 * Constants.NumberOfTilesY) / 2;

        // Load block tile textures
        for (var i = 1; i <= Constants.TileTextureCount; i++)
        {
            _tileTextures[i - 1] = contentManager.Load<Texture2D>($@"tiles\{i}");
        }

        _fixedTileTexture = contentManager.Load<Texture2D>(@"tiles\tile_fixed");

        // Set up the game board
        _gameboardTexture = new Texture2D(Game.GraphicsDevice, 25 * Constants.NumberOfTilesX,
            25 * Constants.NumberOfTilesY);
        var gameboardColorData = new Color[25 * Constants.NumberOfTilesX * 25 * Constants.NumberOfTilesY];
        for (var i = 0; i < gameboardColorData.Length; i++)
        {
            gameboardColorData[i] = Color.FromNonPremultiplied(0, 0, 0, 50);
        }

        _gameboardTexture.SetData(gameboardColorData);
        GameBoard = new GameBoard(this, _gameboardTexture, _fixedTileTexture, Constants.NumberOfTilesX,
            Constants.NumberOfTilesY, _boardX, _boardY);

        // Prepare the tetris queue
        _tetrisQueue.Enqueue(_rnd.Next(_blockGenerator.BlockDefinitionCount));
        _tetrisQueue.Enqueue(_rnd.Next(_blockGenerator.BlockDefinitionCount));

        Subscribe<BlockOverlappedMessage>(BlockOverlappedHandler);

        Add(GameBoard);
    }

    public override void Update(GameTime gameTime)
    {
        var seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var keyState = Keyboard.GetState();

        _timeSinceLastKeyPress += seconds;
        if (_timeSinceLastKeyPress > KeyDelay)
        {
            if (keyState.IsKeyDown(Keys.A))
            {
                _block?.MoveLeft();
            }
            else if (keyState.IsKeyDown(Keys.D))
            {
                _block?.MoveRight();
            }
            else if (keyState.IsKeyDown(Keys.S))
            {
                _block?.MoveDown();
            }
            else if (keyState.HasPressedOnce(Keys.J))
            {
                _block?.Rotate();
            }
            else if (keyState.HasPressedOnce(Keys.K))
            {
                _block?.Drop();
            }
            else if (keyState.HasPressedOnce(Keys.Escape))
            {
                Game.Transit<TitleScene>();
            }

            _timeSinceLastKeyPress = 0;
        }

        base.Update(gameTime);
    }

    #endregion Public Methods

    #region Protected Methods

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _gameboardTexture?.Dispose();
                _fixedTileTexture?.Dispose();
                foreach (var t in _tileTextures)
                {
                    t.Dispose();
                }
            }

            base.Dispose(disposing);
            _disposed = true;
        }
    }

    #endregion Protected Methods

    #region Private Methods

    private void AddBlockToBoard()
    {
        if (_nextBlock is not null)
        {
            Remove(_nextBlock);
        }

        // Dequeue the index of the current block, create the block based
        // on the index and add it to the game board.
        var index = _tetrisQueue.Dequeue();
        _block = _blockGenerator.Create(this, _tileTextures, index, _boardX, _boardY);
        Add(_block);

        // In the meanwhile, prepare for the next block.
        var next = _tetrisQueue.Peek();
        _nextBlock = _blockGenerator.Create(this, _tileTextures, next, _boardX, _boardY, false, 500, 80);
        Add(_nextBlock);

        // Enqueue the index of the next block.
        _tetrisQueue.Enqueue(_rnd.Next(_blockGenerator.BlockDefinitionCount));
    }

    private void BlockOverlappedHandler(object sender, BlockOverlappedMessage message)
    {
        if (message.Checkmate)
        {
        }
        else
        {
            var block = message.Block;

            // Merge the block with the game board.
            GameBoard?.Merge(block.CurrentRotation, (int)block.X, (int)block.Y, () => { });
            var rows = GameBoard?.CleanupFilledRows(rows => { });

            // Remove the current block sprite from the scene.
            Remove(block);

            // And add a new block to the game board.
            AddBlockToBoard();
        }
    }

    private void ResetGame()
    {
        GameBoard?.Reset();
        if (_block is not null)
        {
            Remove(_block);
        }

        if (_nextBlock is not null)
        {
            Remove(_nextBlock);
        }

        AddBlockToBoard();
    }

    #endregion Private Methods
}
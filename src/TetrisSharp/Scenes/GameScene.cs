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
using System.IO;
using FontStashSharp;
using Mfx.Core;
using Mfx.Core.Elements;
using Mfx.Core.Input;
using Mfx.Core.Scenes;
using Mfx.Core.Sounds;
using Mfx.Core.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TetrisSharp.Blocks;
using TetrisSharp.Messages;

namespace TetrisSharp.Scenes;

internal sealed class GameScene : Scene, IGameScene
{

    #region Private Fields

    private const string CopyrightText = "Copyright (C) 2022-2024 by daxnet";
    private const float KeyDelay = 0.09f;
    private static readonly Random _rnd = new(DateTime.Now.Millisecond);

    private readonly BlockGenerator _blockGenerator = new("blocks.xml");
    private readonly FontSystem _mainFontSystem = new();
    private readonly Queue<int> _tetrisQueue = new();
    private readonly Texture2D[] _tileTextures = new Texture2D[Constants.TileTextureCount];
    private Block? _block;
    private int _blocks;
    private int _boardX;
    private int _boardY;
    private bool _disposed;
    private Texture2D? _fixedTileTexture;
    private Texture2D? _gameboardTexture;
    private int _level;
    private int _lines;
    private Block? _nextBlock;
    private int _score;
    private Rectangle _scoreBoardBoundingBox;
    private float _timeSinceLastKeyPress;
    private DynamicSpriteFont? _scoreBoardFont;
    private DynamicSpriteFont? _gameOverFont;
    private SpriteFont? _arialFont;
    private bool _gameOver;
    private Label? _copyrightLabel;
    private Sound? _gameOverSound;
    private SoundEffect? _gameOverSoundEffect;
    private Sound? _mergeSound;
    private SoundEffect? _mergeSoundEffect;
    private Sound? _removeRowSound;
    private SoundEffect? _removeRowSoundEffect;
    private SoundEffect? _bgmEffect;
    private BackgroundMusic? _bgm;

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
        DrawScoreBoard(spriteBatch);
        base.Draw(gameTime, spriteBatch);

        if (_gameOver)
        {
            spriteBatch.DrawString(_gameOverFont, "GAME OVER", new Vector2(_boardX + 12, _boardY + 150), Color.OrangeRed);
        }
    }

    public override void Enter(object? args = null)
    {
        GameAs<TetrisGame>().CanContinue = true;

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

        _bgm?.Play();
    }

    public override void Leave()
    {
        _bgm?.Stop();
    }

    public override void Load(ContentManager contentManager)
    {
        // Board and coordinates
        _boardX = 30;
        _boardY = (Viewport.Height - 25 * Constants.NumberOfTilesY) / 2;

        _scoreBoardBoundingBox = new Rectangle(_boardX + Constants.NumberOfTilesX * 25 + 30, _boardY,
            Viewport.Width - Constants.NumberOfTilesX * 25 - 30 - 2 * _boardX, Viewport.Height - 2 * _boardY);

        // Sound & music
        _gameOverSoundEffect = contentManager.Load<SoundEffect>(@"sounds\gameover");
        _gameOverSound = new(_gameOverSoundEffect, Constants.SoundVolume);
        _mergeSoundEffect = contentManager.Load<SoundEffect>(@"sounds\merge");
        _mergeSound = new(_mergeSoundEffect, Constants.SoundVolume);
        _removeRowSoundEffect = contentManager.Load<SoundEffect>(@"sounds\remove_row");
        _removeRowSound = new(_removeRowSoundEffect, Constants.SoundVolume);
        _bgmEffect = contentManager.Load<SoundEffect>(@"sounds\bgm");
        _bgm = new([_bgmEffect], .2f);

        // Fonts & static texts
        _mainFontSystem.AddFont(File.ReadAllBytes(@"res\main.ttf"));
        _scoreBoardFont = _mainFontSystem.GetFont(38);
        _gameOverFont = _mainFontSystem.GetFont(70);
        _arialFont = contentManager.Load<SpriteFont>(@"fonts\arial");
        var copyrightTextSize = _arialFont.MeasureString(CopyrightText);
        var copyrightTextX = _scoreBoardBoundingBox.X + _scoreBoardBoundingBox.Width - copyrightTextSize.X;
        var copyrightTextY = _scoreBoardBoundingBox.Bottom - copyrightTextSize.Y;
        _copyrightLabel = new Label(CopyrightText, this, _arialFont, copyrightTextX, copyrightTextY, Color.White);

        // Load block tile textures
        for (var i = 1; i <= Constants.TileTextureCount; i++)
        {
            _tileTextures[i - 1] = contentManager.Load<Texture2D>($@"tiles\{i}");
        }

        _fixedTileTexture = contentManager.Load<Texture2D>(@"tiles\tile_fixed");

        // Set up the game board
        _gameboardTexture = CreateGameBoardTexture();
        GameBoard = new GameBoard(this, _gameboardTexture, _fixedTileTexture, Constants.NumberOfTilesX,
            Constants.NumberOfTilesY, _boardX, _boardY);

        // Prepare the tetris queue
        _tetrisQueue.Enqueue(_rnd.Next(_blockGenerator.BlockDefinitionCount));
        _tetrisQueue.Enqueue(_rnd.Next(_blockGenerator.BlockDefinitionCount));

        Subscribe<BlockOverlappedMessage>(BlockOverlappedHandler);

        Add(GameBoard);
        Add(_bgm);
        Add(_copyrightLabel);
    }

    public override void Update(GameTime gameTime)
    {
        var keyState = Keyboard.GetState();
        if (keyState.HasPressedOnce(Keys.Escape))
        {
            Game.Transit<TitleScene>();
        }

        // If game has ended, suppress the response
        // to all key board or joystick events and simply return.
        if (_gameOver)
        {
            return;
        }

        var seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
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

                _gameOverSound?.Stop();
                _gameOverSoundEffect?.Dispose();
                _mergeSound?.Stop();
                _mergeSoundEffect?.Dispose();
                _removeRowSound?.Stop();
                _removeRowSoundEffect?.Dispose();
                _bgm?.Stop();
                _bgmEffect?.Dispose();
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
        _block.FallingInterval = TimeSpan.FromMilliseconds(Math.Max(1000 - (_level - 1) * 50, 1));

        Add(_block);
        _blocks++;

        // In the meanwhile, prepare for the next block.
        var next = _tetrisQueue.Peek();
        _nextBlock = _blockGenerator.Create(this, _tileTextures, next, _boardX, _boardY, false,
            _scoreBoardBoundingBox.X, 435);

        Add(_nextBlock);

        // Enqueue the index of the next block.
        _tetrisQueue.Enqueue(_rnd.Next(_blockGenerator.BlockDefinitionCount));
    }

    private void BlockOverlappedHandler(object sender, BlockOverlappedMessage message)
    {
        if (message.Checkmate)
        {
            _bgm?.Stop();
            _gameOverSound?.Play();
            _gameOver = true;
        }
        else
        {
            var block = message.Block;

            // Merge the block with the game board.
            GameBoard?.Merge(block.CurrentRotation, (int)block.X, (int)block.Y, () => _mergeSound?.Play());
            _score++;

            var rows = GameBoard?.CleanupFilledRows(_ => _removeRowSound?.Play());
            _score += (rows ?? 0) * 50;
            _lines += rows ?? 0;

            // Recalculate level
            if (_score > _level * 2000)
            {
                _level++;
            }

            // Remove the current block sprite from the scene.
            Remove(block);

            // And add a new block to the game board.
            AddBlockToBoard();
        }
    }

    private Texture2D CreateGameBoardTexture()
    {
        var gameboardTexture = new Texture2D(Game.GraphicsDevice, 25 * Constants.NumberOfTilesX,
            25 * Constants.NumberOfTilesY);
        var gameboardColorData = new Color[25 * Constants.NumberOfTilesX * 25 * Constants.NumberOfTilesY];
        for (var i = 0; i < gameboardColorData.Length; i++)
        {
            gameboardColorData[i] = Color.FromNonPremultiplied(0, 0, 0, 50);
        }

        gameboardTexture.SetData(gameboardColorData);
        return gameboardTexture;
    }

    private void DrawScoreBoard(SpriteBatch spriteBatch)
    {
        // Score
        spriteBatch.DrawString(_scoreBoardFont, "Score", new Vector2(_scoreBoardBoundingBox.X, _boardY), Color.White);
        spriteBatch.DrawString(_scoreBoardFont, _score.ToString().PadLeft(9, '0'),
            new Vector2(_scoreBoardBoundingBox.X, _boardY + 30), Color.YellowGreen * 0.9f);

        // Level
        spriteBatch.DrawString(_scoreBoardFont, "Level", new Vector2(_scoreBoardBoundingBox.X, _boardY + 90), Color.White);
        spriteBatch.DrawString(_scoreBoardFont, _level.ToString().PadLeft(2, '0'),
            new Vector2(_scoreBoardBoundingBox.X, _boardY + 120), Color.YellowGreen * 0.9f);

        // Blocks
        spriteBatch.DrawString(_scoreBoardFont, "Blocks", new Vector2(_scoreBoardBoundingBox.X, _boardY + 180), Color.White);
        spriteBatch.DrawString(_scoreBoardFont, _blocks.ToString().PadLeft(5, '0'),
            new Vector2(_scoreBoardBoundingBox.X, _boardY + 210), Color.YellowGreen * 0.9f);

        // Lines
        spriteBatch.DrawString(_scoreBoardFont, "Lines", new Vector2(_scoreBoardBoundingBox.X, _boardY + 270), Color.White);
        spriteBatch.DrawString(_scoreBoardFont, _lines.ToString().PadLeft(5, '0'),
            new Vector2(_scoreBoardBoundingBox.X, _boardY + 300), Color.YellowGreen * 0.9f);

        spriteBatch.DrawString(_scoreBoardFont, "Next", new Vector2(_scoreBoardBoundingBox.X, _boardY + 360), Color.White);
    }
    private void ResetGame()
    {
        _score = 0;
        _level = 1;
        _lines = 0;
        _blocks = 0;
        _gameOver = false;

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
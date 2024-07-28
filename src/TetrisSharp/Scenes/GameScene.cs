using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Mfx.Core;
using Mfx.Core.Input;
using Mfx.Core.Scenes;
using Mfx.Core.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TetrisSharp.Blocks;
using TetrisSharp.Messages;

namespace TetrisSharp.Scenes
{
    internal sealed class GameScene : Scene, IGameScene
    {
        private const float KeyDelay = 0.08f;
        private static readonly Random _rnd = new(DateTime.Now.Millisecond);

        private readonly BlockGenerator _blockGenerator = new("blocks.xml");
        private readonly Texture2D[] _tileTextures = new Texture2D[Constants.TileTextureCount];
        private Texture2D? _fixedTileTexture;
        private readonly Queue<int> _tetrisQueue = new();

        private Texture2D? _gameboardTexture;
        private bool _disposed;
        private int _boardX;
        private int _boardY;
        private Block? _block;
        private Block? _nextBlock;
        private float _timeSinceLastKeyPress;

        public GameScene(TetrisGame game, string name) 
            : base(game, name, Color.Gray)
        {
            
        }

        public override void Enter(object? args = null)
        {
            if (Game is TetrisGame tg)
            {
                tg.CanContinue = true;
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
                gameboardColorData[i] = Color.Black;
            }
            _gameboardTexture.SetData(gameboardColorData);
            GameBoard = new GameBoard(this, _gameboardTexture, _fixedTileTexture, Constants.NumberOfTilesX,
                Constants.NumberOfTilesY, _boardX, _boardY);

            // Prepare the tetris queue
            _tetrisQueue.Enqueue(_rnd.Next(_blockGenerator.BlockDefinitionCount));
            _tetrisQueue.Enqueue(_rnd.Next(_blockGenerator.BlockDefinitionCount));

            Subscribe<BlockOverlappedMessage>(BlockOverlappedHandler);

            Add(GameBoard);
            AddBlockToBoard();
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
                else if (keyState.HasPressedOnce(Keys.Escape))
                {
                    Game.Transit<TitleScene>();
                }
                _timeSinceLastKeyPress = 0;
            }

            base.Update(gameTime);
        }

        public GameBoard? GameBoard { get; private set; }

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
                var rows = GameBoard?.CleanupFilledRows(row => { });

                // Remove the current block sprite from the scene.
                Remove(block);

                // And add a new block to the game board.
                AddBlockToBoard();
            }
        }

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

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_gameboardTexture, new Vector2(_boardX, _boardY), Color.White);
            base.Draw(gameTime, spriteBatch);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _gameboardTexture?.Dispose();
                }

                base.Dispose(disposing);
                _disposed = true;
            }
        }
    }
}

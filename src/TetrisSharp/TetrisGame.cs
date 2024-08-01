using Mfx.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TetrisSharp.Scenes;

namespace TetrisSharp
{
    public class TetrisGame : MfxGame
    {
        public TetrisGame()
            : base(MfxGameSettings.FromDefault("Tetris#", MfxGameSettings.NormalScreenFixedSizeShowMouse,
                new Point(820, 768)))
        {
            AddScene<TitleScene>();
            AddScene<GameScene>();
            StartFrom<TitleScene>();
        }

        public bool CanContinue { get; set; } = false;
    }
}

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
            : base(MfxGameSettings.DefaultWithTitle("Tetris#", MfxGameSettings.NormalScreenFixedSizeShowMouse))
        {
            AddScene<TitleScene>();
            AddScene<MainScene>();
            StartFrom<TitleScene>();
        }
    }
}

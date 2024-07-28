using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mfx.Core.Scenes;

namespace TetrisSharp.Scenes
{
    internal interface IGameScene : IScene
    {
        GameBoard? GameBoard { get; }
    }
}

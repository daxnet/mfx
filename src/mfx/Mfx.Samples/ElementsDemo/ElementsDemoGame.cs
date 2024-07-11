using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mfx.Core;

namespace Mfx.Samples.ElementsDemo
{
    internal class ElementsDemoGame : MfxGame
    {
        public ElementsDemoGame()
            : base(MfxGameSettings.DefaultWithTitle("Elements Demonstration"))
        {
            FirstScene = AddScene<ElementsDemoScene>("ElementsDemoScene");
            AddScene<NewGameScene>("NewGameScene", FirstScene);
        }
    }
}

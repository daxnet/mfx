using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mfx.Core
{
    public interface IDrawable
    {
        

        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}

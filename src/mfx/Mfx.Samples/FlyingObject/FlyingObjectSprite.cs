using Mfx.Core.Scenes;
using Mfx.Core.Sprites;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mfx.Core;
using Mfx.Core.Messaging;
using Microsoft.Xna.Framework;

namespace Mfx.Samples.FlyingObject
{
    internal sealed class FlyingObjectSprite : Sprite
    {
        // ReSharper disable once InconsistentNaming
        private static readonly Random _rnd = new Random(DateTime.UtcNow.Millisecond);

        private int _dx;
        private int _dy;

        public FlyingObjectSprite(IScene scene, Texture2D texture, int x, int y, int dx, int dy)
            : base(scene, texture, x, y)
        {
            _dx = dx;
            _dy = dy;
            Subscribe<BoundaryHitMessage>((p, m) =>
            {
                if (Equals(p))
                {
                    if ((m.Boundary & Boundary.Left) == Boundary.Left ||
                        (m.Boundary & Boundary.Right) == Boundary.Right)
                    {
                        _dx *= -1;
                    }

                    if ((m.Boundary & Boundary.Top) == Boundary.Top ||
                        (m.Boundary & Boundary.Bottom) == Boundary.Bottom)
                    {
                        _dy *= -1;
                    }
                }
            });

            EnableBoundaryDetection = true;
        }

        public override void Update(GameTime gameTime)
        {
            X += _dx;
            Y += _dy;
            base.Update(gameTime);
        }
    }
}

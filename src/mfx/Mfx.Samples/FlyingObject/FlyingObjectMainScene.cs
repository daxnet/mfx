using Mfx.Core;
using Mfx.Core.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Mfx.Samples.FlyingObject
{
    internal sealed class FlyingObjectMainScene(MfxGame game) : Scene(game, Color.Black)
    {
        // ReSharper disable once InconsistentNaming
        private static readonly Random _rnd = new(DateTime.UtcNow.Millisecond);
        private Texture2D? _spriteTexture;
        private bool _disposed;

        public override void Load(ContentManager contentManager)
        {
            var screenWidth = Game.GraphicsDevice.Viewport.Width;
            var screenHeight = Game.GraphicsDevice.Viewport.Height;
            _spriteTexture = contentManager.Load<Texture2D>("american-football");
            var initialX = _rnd.Next(1, screenWidth - _spriteTexture.Width);
            var initialY = _rnd.Next(1, screenHeight - _spriteTexture.Height);
            var objectSprite = new FlyingObjectSprite(this, _spriteTexture, initialX, initialY, 5, 5);
            Add(objectSprite);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _spriteTexture?.Dispose();
                }

                base.Dispose(disposing);
                _disposed = true;
            }
        }
    }
}

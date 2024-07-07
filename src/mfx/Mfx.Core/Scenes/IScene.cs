using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Mfx.Core.Scenes
{
    public interface IScene : ICollection<IComponent>, IVisibleComponent, IDisposable
    {
        Color BackgroundColor { get; }

        MfxGame Game { get; }

        IScene? Next { get; set; }

        void Enter();

        void Leave();

        void Load(ContentManager contentManager);
    }
}

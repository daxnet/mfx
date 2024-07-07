using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mfx.Core.Messaging;

namespace Mfx.Core.Scenes
{
    public sealed class SceneEndedMessage(IScene scene) : Message
    {
        public IScene Scene => scene;
    }
}

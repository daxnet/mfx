using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mfx.Core.Messaging;

namespace Mfx.Core.Physics
{
    public sealed class OutOfViewportMessage(IComponent component) : Message
    {
        public IComponent Component { get; } = component;
    }
}

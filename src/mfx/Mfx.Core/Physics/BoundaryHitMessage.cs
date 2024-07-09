using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mfx.Core.Messaging;

namespace Mfx.Core.Physics
{
    public sealed class BoundaryHitMessage(Boundary boundary) : Message
    {
        public Boundary Boundary { get; } = boundary;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mfx.Core.Messaging
{
    public sealed class BoundaryHitMessage(Boundary boundary) : Message
    {
        public Boundary Boundary { get; } = boundary;
    }
}

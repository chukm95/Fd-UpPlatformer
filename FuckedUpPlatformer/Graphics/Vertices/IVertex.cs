using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuckedUpPlatformer.Graphics.Vertices {
    internal interface IVertex {
        public static VertexComponentDescription[] Description { get; }
    }
}

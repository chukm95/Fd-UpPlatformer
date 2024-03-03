using OpenTK.Mathematics;
using System.Runtime.InteropServices;

namespace FuckedUpPlatformer.Graphics.Vertices {
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    internal struct VertexPositionTexture {
        {
        public static VertexComponentDescription[] Description => new VertexComponentDescription[]
        {
            new VertexComponentDescription(VertexComponetType.VEC3, 0, 0),
            new VertexComponentDescription(VertexComponetType.VEC2, 12, 1)
        };

        public Vector3 Position;
        public Vector2 TextCoord;
    }
}

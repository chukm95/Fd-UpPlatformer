using OpenTK.Mathematics;
using System.Runtime.InteropServices;

namespace FuckedUpPlatformer.Graphics.Vertices
{
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    internal struct VertexPositionTextureColor : IVertex
    {
        public static VertexComponentDescription[] Description => new VertexComponentDescription[]
        {
            new VertexComponentDescription(VertexComponetType.VEC3, 0, 0),
            new VertexComponentDescription(VertexComponetType.VEC4, 12, 1),
            new VertexComponentDescription(VertexComponetType.VEC2, 28, 2)
        };

        public Vector3 Position;
        public Vector4 Color;
        public Vector2 TexCoord;
    }
}

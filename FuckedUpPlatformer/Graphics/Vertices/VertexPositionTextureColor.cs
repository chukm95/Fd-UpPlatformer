using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace FuckedUpPlatformer.Graphics.Vertices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VertexPositionTextureColor
    {
        public static readonly ReadOnlyCollection<VertexDescription> VertexDescription =
            new VertexDescription[]
            {
                new VertexDescription(VertexComponentTypes.VEC3, 0),
                new VertexDescription(VertexComponentTypes.VEC2, 2),
                new VertexDescription(VertexComponentTypes.VEC4, 3),
                new VertexDescription(VertexComponentTypes.FLOAT, 4)
            }.AsReadOnly();

        public Vector3 Position;
        public Vector2 UV;
        public Vector4 Color;
        public float textureChannel;
    }
}
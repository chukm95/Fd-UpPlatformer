using OpenTK.Mathematics;

namespace FuckedUpPlatformer.Graphics.Vertices
{
    public struct VertexDescription
    {
        public readonly VertexComponentTypes Type;
        public readonly int LayoutBinding;

        public VertexDescription(VertexComponentTypes vertexComponentType, int layoutBinding) {
            Type = vertexComponentType;
            LayoutBinding = layoutBinding;
        }
    }
}
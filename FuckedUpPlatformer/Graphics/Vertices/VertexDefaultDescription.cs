using OpenTK.Mathematics;

namespace FuckedUpPlatformer.Graphics.Vertices
{
    public struct VertexDefaultDescription
    {
        public readonly VertexComponentTypes Type;
        public readonly int LayoutBinding;
        public readonly dynamic DefaultValue;

        public VertexDefaultDescription(int layoutBinding, float defaultValue) {
            Type = VertexComponentTypes.FLOAT;
            LayoutBinding = layoutBinding;
            DefaultValue = defaultValue;
        }

        public VertexDefaultDescription(int layoutBinding, Vector2 defaultValue) {
            Type = VertexComponentTypes.VEC2;
            LayoutBinding = layoutBinding;
            DefaultValue = defaultValue;
        }

        public VertexDefaultDescription(int layoutBinding, Vector3 defaultValue) {
            Type = VertexComponentTypes.VEC3;
            LayoutBinding = layoutBinding;
            DefaultValue = defaultValue;
        }

        public VertexDefaultDescription(int layoutBinding, Vector4 defaultValue) {
            Type = VertexComponentTypes.VEC4;
            LayoutBinding = layoutBinding;
            DefaultValue = defaultValue;
        }

        public VertexDefaultDescription(int layoutBinding, Matrix4 defaultValue) {
            Type = VertexComponentTypes.MAT4;
            LayoutBinding = layoutBinding;
            DefaultValue = defaultValue;
        }
    }
}

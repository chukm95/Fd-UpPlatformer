namespace FuckedUpPlatformer.Graphics.Vertices
{
    internal enum VertexComponetType
    {
        FLOAT,
        VEC2,
        VEC3,
        VEC4,
        MAT4
    }

    internal struct VertexComponentDescription
    {
        public readonly VertexComponetType _componentType;
        public readonly int _componentOffset;
        public readonly int _componentBinding;

        public VertexComponentDescription(VertexComponetType componentType, int componentOffset, int componentBinding)
        {
            _componentType = componentType;
            _componentOffset = componentOffset;
            _componentBinding = componentBinding;
        }
    }
}

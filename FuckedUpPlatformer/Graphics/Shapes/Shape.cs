using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FuckedUpPlatformer.Graphics.Shapes
{
    internal class Shape
    {
        public ReadOnlyCollection<Vector3> Positions => _positions;
        public ReadOnlyCollection<Vector2> UVs => _uvs;
        public ReadOnlyCollection<uint> Indices => _indices;

        private readonly ReadOnlyCollection<Vector3> _positions;
        private readonly ReadOnlyCollection<Vector2> _uvs;
        private readonly ReadOnlyCollection<uint> _indices;

        public Shape(Vector3[] positions, Vector2[] uvs, uint[] indices)
        {
            _positions = positions.AsReadOnly();
            _uvs = uvs.AsReadOnly();
            _indices = indices.AsReadOnly();
        }
    }
}
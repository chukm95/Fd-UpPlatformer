using FuckedUpPlatformer.Graphics.Buffers;
using FuckedUpPlatformer.Graphics.Vertices;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;

namespace FuckedUpPlatformer.Graphics.Batching {
    internal class QuadBatcher {

        private VertexPositionColor[] _templateVertices = new VertexPositionColor[] {
            new VertexPositionColor{ Position = new Vector3(-32, 32, 0), Color = (Vector4)Color4.White},
            new VertexPositionColor{ Position = new Vector3(32, 32, 0), Color = (Vector4)Color4.White},
            new VertexPositionColor{ Position = new Vector3(32, -32, 0), Color = (Vector4)Color4.White},
            new VertexPositionColor{ Position = new Vector3(-32, -32, 0), Color = (Vector4)Color4.White}
        };

        private uint[] _templateIndices = new uint[] {
            0, 1, 2, 0, 2, 3
        };

        private bool _isDisposed;
        private bool _isBatching;
        private VertexPositionColor[] _vertices;
        private uint[] _indices;
        private int _allocationStrideInQuads;
        private BufferUsageHint _bufferUsageHint;
        private int _nrOfElements;
        private int _nrOfVertices;
        private GraphicsBuffer<VertexPositionColor, uint> _gb;



        public QuadBatcher(int allocationStride, BufferUsageHint bufferUsageHint) {
            _isDisposed = false;
            _isBatching = false;
            _bufferUsageHint = bufferUsageHint;
            _allocationStrideInQuads = allocationStride;
            _indices = new uint[allocationStride];
            _vertices = new VertexPositionColor[allocationStride];
            _nrOfElements = 0;
            _nrOfVertices = 0;
            _gb = new GraphicsBuffer<VertexPositionColor, uint>(PrimitiveType.Triangles, VertexPositionColor.Description, _bufferUsageHint, Marshal.SizeOf<VertexPositionColor>());
        }

        public void Begin() {
            if (_isBatching) throw new Exception($"Already Batching!");

            _isBatching = true;
            _nrOfElements = 0;
            _nrOfVertices = 0;

        }

        public void Batch(Vector3 position, Vector3 scale, Vector3 rotation, Color4 color) {
            if (!_isBatching) throw new Exception($"Can't Batch Bitch!");

            if (_nrOfElements + _templateIndices.Length >= _indices.Length) {
                Array.Resize(ref _indices, _indices.Length + (_allocationStrideInQuads * _templateIndices.Length));
            }
            if (_nrOfVertices + _templateVertices.Length >= _vertices.Length) {
                Array.Resize(ref _vertices, _vertices.Length + (_allocationStrideInQuads * _templateVertices.Length));
            }

            Matrix4 transform = Matrix4.CreateScale(scale) * Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(rotation)) * Matrix4.CreateTranslation(position);

            for (int i = 0; i < _templateVertices.Length; i++) {
                ref VertexPositionColor v = ref _vertices[_nrOfVertices + i];
                VertexPositionColor vt = _templateVertices[i];
                v.Position = Vector3.TransformPosition(vt.Position, transform);
                v.Color = vt.Color * (Vector4)color;
            }

            for (int i = 0; i < _templateIndices.Length; i++) {
                _indices[_nrOfElements + i] = _templateIndices[i] + (uint)_nrOfVertices;
            }

            _nrOfVertices += _templateVertices.Length;
            _nrOfElements += _templateIndices.Length;
        }

        public void End() {
            if (!_isBatching) throw new Exception($"Can't End Batch Bitch!");
            _gb.BufferVertices(_vertices);
            _gb.BufferIndices(_indices);
            _isBatching = false;
        }

        public void Draw() {
            _gb.Draw(_nrOfElements);
        }

        public void Dispose() {
            if (!_isDisposed) throw new Exception($"Can't dispose this!");
            _isDisposed = true;
            _gb.Dispose();
        }

    }

}

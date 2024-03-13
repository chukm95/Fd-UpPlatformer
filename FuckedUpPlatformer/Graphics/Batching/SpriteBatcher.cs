using FuckedUpPlatformer.Graphics.Buffers;
using FuckedUpPlatformer.Graphics.Shapes;
using FuckedUpPlatformer.Graphics.Vertices;
using FuckedUpPlatformer.Resources.Textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace FuckedUpPlatformer.Graphics.Batching
{
    internal class SpriteBatcher
    {
        private static VertexDefaultDescription[] VERTEX_DEFAULT_DESCRIPTIONS = new VertexDefaultDescription[]
        {
            new VertexDefaultDescription(0, Vector3.Zero),
                new VertexDefaultDescription(1, Vector3.Zero),
                new VertexDefaultDescription(2, Vector2.Zero),
                new VertexDefaultDescription(3, (Vector4)Color4.White),
                new VertexDefaultDescription(4, 16f),
                new VertexDefaultDescription(5, Matrix4.Identity)
        };
        private readonly Vector4 DEFAULT_FRAME = new Vector4(0, 0, 1, 1);
        private readonly int DEFAULT_TEXTURE_CHANNEL = 16;

        private VertexBufferObject<VertexPositionTextureColor> _vertexBufferObject;
        private ElementBufferObject<uint> _elementBufferObject;
        private VertexArrayObject _vertexArrayObject;

        private VertexPositionTextureColor[] _vertices;
        private uint[] _indices;

        private readonly int _strideCount;
        private int _numOfVertices;
        private int _numOfIndices;
        private int _numOfElements;

        private Dictionary<string, (int channel, Texture texture)> _usedTextures;
        private Shape _defaultShape;

        private bool _isBatching;
        private bool _isDisposed;

        public SpriteBatcher(int strideCount, BufferUsageHint bufferUsageHint) {
            _strideCount = strideCount;

            _vertexBufferObject = new VertexBufferObject<VertexPositionTextureColor>(VertexPositionTextureColor.VertexDescription, bufferUsageHint);
            _elementBufferObject = new ElementBufferObject<uint>(bufferUsageHint);
            _vertexArrayObject = new VertexArrayObject(_vertexBufferObject, _elementBufferObject, PrimitiveType.Triangles, VERTEX_DEFAULT_DESCRIPTIONS);

            _vertices = new VertexPositionTextureColor[strideCount];
            _indices = new uint[strideCount];
            _numOfVertices = 0;

            _usedTextures = new Dictionary<string, (int, Texture)>();
            _defaultShape = ShapeGenerator.GenerateCenteredQuad();

            _isBatching = false;
            _isDisposed = false;
        }

        public void Begin() {
            if (_isBatching) throw new Exception("SpriteBatcher is already batching!");
            if (_isDisposed) throw new Exception("SpriteBatcher is disposed!");

            _numOfVertices = 0;
            _numOfIndices = 0;
            _numOfElements = 0;
            _isBatching = true;
        }

        public void Batch(Shape shape, Vector3 position, Color4 color) {
            Batch(shape, position, color, Vector3.One, Vector3.Zero, Vector3.Zero, null, DEFAULT_FRAME);
        }

        public void Batch(Vector3 position, Color4 color) {
            Batch(_defaultShape, position, color, Vector3.One, Vector3.Zero, Vector3.Zero, null, DEFAULT_FRAME);
        }

        public void Batch(Shape shape, Vector3 position, Color4 color, Vector3 scale) {
            Batch(shape, position, color, scale, Vector3.Zero, Vector3.Zero, null, DEFAULT_FRAME);
        }

        public void Batch(Vector3 position, Color4 color, Vector3 scale) {
            Batch(_defaultShape, position, color, scale, Vector3.Zero, Vector3.Zero, null, DEFAULT_FRAME);
        }

        public void Batch(Shape shape, Vector3 position, Color4 color, Vector3 scale, Vector3 rotation) {
            Batch(shape, position, color, scale, rotation, Vector3.Zero, null, DEFAULT_FRAME);
        }

        public void Batch(Vector3 position, Color4 color, Vector3 scale, Vector3 rotation) {
            Batch(_defaultShape, position, color, scale, rotation, Vector3.Zero, null, DEFAULT_FRAME);
        }

        public void Batch(Shape shape, Vector3 position, Color4 color, Vector3 scale, Vector3 rotation, Vector3 offset) {
            Batch(shape, position, color, scale, rotation, offset, null, DEFAULT_FRAME);
        }

        public void Batch(Vector3 position, Color4 color, Vector3 scale, Vector3 rotation, Vector3 offset) {
            Batch(_defaultShape, position, color, scale, rotation, offset, null, DEFAULT_FRAME);
        }

        public void Batch(Vector3 position, Color4 color, Vector3 scale, Vector3 rotation, Vector3 offset, Texture texture) {
            Batch(_defaultShape, position, color, scale, rotation, offset, texture, DEFAULT_FRAME);
        }

        public void Batch(Shape shape, Vector3 position, Color4 color, Vector3 scale, Vector3 rotation, Vector3 offset, Texture texture) {
            Batch(shape, position, color, scale, rotation, offset, texture, DEFAULT_FRAME);
        }

        public void Batch(Vector3 position, Color4 color, Vector3 scale, Vector3 rotation, Vector3 offset, Texture texture, Vector4 frame) {
            Batch(_defaultShape, position, color, scale, rotation, offset, texture, frame);
        }

        public void Batch(Shape shape, Vector3 position, Color4 color, Vector3 scale, Vector3 rotation, Vector3 offset, Texture texture, Vector4 frame) {
            if (!_isBatching) throw new Exception("SpriteBatcher is not batching!");
            if (_isDisposed) throw new Exception("SpriteBatcher is disposed!");

            ResizeCheck(ref _vertices, _numOfVertices + shape.Positions.Count);
            ResizeCheck(ref _indices, _numOfIndices + shape.Indices.Count);

            Matrix4 transform = Matrix4.CreateScale(scale) * Matrix4.CreateTranslation(offset) * Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(rotation)) * Matrix4.CreateTranslation(position);
            int textureChannel = CacheTexture(texture);

            for (int i = 0; i < shape.Positions.Count; i++) {
                ref VertexPositionTextureColor vertex = ref _vertices[_numOfVertices + i];
                vertex.Position = Vector3.TransformPosition(shape.Positions[i], transform);
                vertex.UV.X = frame.X + shape.UVs[i].X * frame.Z;
                vertex.UV.Y = frame.Y + shape.UVs[i].Y * frame.W;
                vertex.Color = (Vector4)color;
                vertex.textureChannel = textureChannel;
            }

            for (int i = 0; i < shape.Indices.Count; i++) {
                _indices[_numOfIndices + i] = shape.Indices[i] + (uint)_numOfVertices;
            }

            _numOfVertices += shape.Positions.Count;
            _numOfIndices += shape.Indices.Count;
            _numOfElements += shape.Indices.Count / 3;
        }

        public void End() {
            if (!_isBatching) throw new Exception("SpriteBatcher is not batching!");
            if (_isDisposed) throw new Exception("SpriteBatcher is disposed!");

            _isBatching = false;
            _vertexBufferObject.Buffer(_vertices);
            _elementBufferObject.Buffer(_indices);
        }

        public void Draw() {
            if (_isDisposed) throw new Exception("SpriteBatcher is disposed!");
            foreach (var texBind in _usedTextures.Values)
                texBind.texture.Bind(texBind.channel);
            _vertexArrayObject.Draw(_numOfIndices);
        }

        private void ResizeCheck<T>(ref T[] array, int newDataCount) {
            int elementsShortage = newDataCount - array.Length;

            if (elementsShortage >= 0) {
                int newArrayLength = (int)MathF.Ceiling(elementsShortage / (float)_strideCount);
                Array.Resize(ref array, array.Length + newArrayLength * _strideCount);
            }
        }

        private int CacheTexture(Texture texture) {
            if (texture == null || _usedTextures.Count > 15)
                return DEFAULT_TEXTURE_CHANNEL;

            if (!_usedTextures.ContainsKey(texture.FilePath)) {
                int textureCount = _usedTextures.Count;
                _usedTextures.Add(texture.FilePath, (textureCount, texture));
                return textureCount;
            }
            else
                return _usedTextures[texture.FilePath].Item1;
        }

        public void Dispose() {
            if (_isDisposed) throw new Exception("SpriteBatcher is disposed!");
            _isDisposed = true;
            _vertexBufferObject.Dispose();
            _vertexBufferObject = null;
            _elementBufferObject.Dispose();
            _elementBufferObject = null;
            _vertexArrayObject.Dispose();
            _vertexArrayObject = null;
        }
    }
}
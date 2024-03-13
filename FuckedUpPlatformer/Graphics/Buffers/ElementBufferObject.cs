using OpenTK.Graphics.OpenGL4;

namespace FuckedUpPlatformer.Graphics.Buffers
{
    public class ElementBufferObject<I> : IElementBufferObject where I : struct
    {
        public BufferUsageHint BufferUsageHint => _bufferUsageHint;
        public DrawElementsType DrawElementsType => _elementType;
        public int ElementSizeInBytes => _elementSize;
        public bool IsDisposed => _disposed;

        private readonly BufferUsageHint _bufferUsageHint;
        private readonly DrawElementsType _elementType;
        private readonly int _elementSize;
        private int _ebo;
        private bool _disposed;

        public ElementBufferObject(BufferUsageHint bufferUsageHint) {
            _bufferUsageHint = bufferUsageHint;
            InitializeDrawElementType(out _elementType, out _elementSize);
            _ebo = GL.GenBuffer();
            _disposed = false;
        }

        private void InitializeDrawElementType(out DrawElementsType elementType, out int elementSize) {
            var indexType = typeof(I);

            if (indexType == typeof(byte)) {
                elementType = DrawElementsType.UnsignedByte;
                elementSize = sizeof(byte);
                return;
            }
            else if (indexType == typeof(short)) {
                elementType = DrawElementsType.UnsignedShort;
                elementSize = sizeof(short);
                return;
            }
            else if (indexType == typeof(uint)) {
                elementType = DrawElementsType.UnsignedInt;
                elementSize = sizeof(uint);
                return;
            }

            throw new System.Exception($"Indexbuffer invalid element type {indexType}!");
        }

        public void Bind() {
            if (_disposed) return;

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        }

        public void Buffer(I[] indices) {
            if (_disposed) return;

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _elementSize * indices.Length, indices, _bufferUsageHint);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        public void Dispose() {
            if (_disposed) return;

            GL.DeleteBuffer(_ebo);
            _ebo = -1;
            _disposed = true;
        }
    }
}
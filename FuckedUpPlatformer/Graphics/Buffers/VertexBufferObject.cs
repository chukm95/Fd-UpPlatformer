using FuckedUpPlatformer.Graphics.Vertices;
using OpenTK.Graphics.OpenGL4;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace FuckedUpPlatformer.Graphics.Buffers
{
    public class VertexBufferObject<V> : IVertexBufferObject where V : struct
    {
        public ReadOnlyCollection<VertexDescription> VertexDescription => _vertexDescriptions;
        public BufferUsageHint BufferUsageHint => _bufferUsageHint;
        public int VertexSizeInBytes => _vertexSizeInBytes;
        public bool IsDisposed => _disposed;

        private readonly ReadOnlyCollection<VertexDescription> _vertexDescriptions;
        private readonly BufferUsageHint _bufferUsageHint;
        private readonly int _vertexSizeInBytes;
        private int _vbo;
        private bool _disposed;

        public VertexBufferObject(ReadOnlyCollection<VertexDescription> vertexDescription, BufferUsageHint bufferUsageHint)
        {
            _vertexDescriptions = vertexDescription;
            _bufferUsageHint = bufferUsageHint;
            _vertexSizeInBytes = Marshal.SizeOf<V>();
            _vbo = GL.GenBuffer();
            _disposed = false;
        }

        public void Bind()
        {
            if (_disposed) return;

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        }

        public void Buffer(V[] vertices)
        {
            if (_disposed) return;

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertexSizeInBytes * vertices.Length, vertices, _bufferUsageHint);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void Dispose()
        {
            if (_disposed) return;

            GL.DeleteBuffer(_vbo);
            _vbo = -1;
            _disposed = true;
        }
    }
}
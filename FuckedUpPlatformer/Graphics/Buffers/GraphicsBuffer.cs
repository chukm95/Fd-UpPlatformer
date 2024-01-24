using FuckedUpPlatformer.Graphics.Vertices;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace FuckedUpPlatformer.Graphics.Buffers
{
    internal class GraphicsBuffer<V, I> where V : struct, IVertex where I : struct
    {
        private DrawElementsType _drawElementsType;
        private PrimitiveType _primitiveType;
        private VertexComponentDescription[] _vertexComponentDescriptions;
        private BufferUsageHint _bufferusagehint;
        private int _vertexSize;
        private int _indexSize;

        private List<int> _vertexAttributes;

        private int _vertexArrayObjectId;
        private int _indexBufferId;
        private int _vertexBufferId;

        private bool _isDisposed;

        public GraphicsBuffer(PrimitiveType primitiveType, VertexComponentDescription[] descriptions, BufferUsageHint bufferUsageHint, int vertexSize) 
        { 
            _drawElementsType = GetDrawElementType();
            _primitiveType = primitiveType;
            _vertexComponentDescriptions = descriptions;
            _bufferusagehint = bufferUsageHint;
            _vertexSize = vertexSize;

            _vertexAttributes = new List<int>();

            _vertexArrayObjectId = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObjectId);

            _indexBufferId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indexBufferId);

            _vertexBufferId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferId);
            InitalizeVertexBufferDescriptions();

            GL.BindVertexArray(0);

            _isDisposed = false;
        }

        private DrawElementsType GetDrawElementType()
        {
            Type indexType = typeof(I);

            if (indexType == typeof(uint))
            {
                _indexSize = 4;
                return DrawElementsType.UnsignedInt;
            }
            else if (indexType == typeof(ushort))
            {
                _indexSize = 2;
                return DrawElementsType.UnsignedShort;
            }
            else if (indexType == typeof(byte))
            {
                _indexSize = 1;
                return DrawElementsType.UnsignedByte;
            }
            else
                throw new Exception("Wrong DrawElementType for graphicsBuffer");
        }

        private void InitalizeVertexBufferDescriptions()
        {
            foreach (var compDesc in _vertexComponentDescriptions)
            {
                switch (compDesc._componentType)
                {
                    case VertexComponetType.VEC3:
                        GL.VertexAttribPointer(compDesc._componentBinding, 3, VertexAttribPointerType.Float, false, _vertexSize, compDesc._componentOffset);
                        break;
                    case VertexComponetType.VEC4:
                        GL.VertexAttribPointer(compDesc._componentBinding, 4, VertexAttribPointerType.Float, false, _vertexSize, compDesc._componentOffset);
                        break;
                }

                _vertexAttributes.Add(compDesc._componentBinding);
            }
        }

        public void BufferVertices(V[] vertices)
        {
            if (_isDisposed) return;

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertexSize * vertices.Length, vertices, _bufferusagehint);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void BufferIndices(I[] indices)
        {
            if (_isDisposed) return;

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indexBufferId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indexSize * indices.Length, indices, _bufferusagehint);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        public void Draw(int indices)
        {
            GL.BindVertexArray(_vertexArrayObjectId);

            foreach (int i in _vertexAttributes)
                GL.EnableVertexAttribArray(i);

            GL.DrawElements(_primitiveType, indices, _drawElementsType, IntPtr.Zero);

            foreach (int i in _vertexAttributes)
                GL.DisableVertexAttribArray(i);

            GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            if(_isDisposed) return;

            GL.DeleteBuffer(_indexBufferId);
            GL.DeleteBuffer(_vertexBufferId);
            GL.DeleteVertexArray(_vertexArrayObjectId);
            _isDisposed = true;
        }
    }
}

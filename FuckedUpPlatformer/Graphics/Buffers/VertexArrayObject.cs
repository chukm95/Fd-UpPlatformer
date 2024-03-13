using FuckedUpPlatformer.Graphics.Vertices;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System;

namespace FuckedUpPlatformer.Graphics.Buffers
{
    public enum DrawMethod
    {
        DRAW_ARRAYS,
        DRAW_ELEMENTS,
        DRAW_ARRAYS_INSTANCED,
        DRAW_ELEMENTS_INSTANCED
    }

    public class VertexArrayObject
    {
        private PrimitiveType _primitiveType;
        private DrawElementsType _drawElementType;
        private DrawMethod _drawMethod;
        private int _vertexArrayObject;
        private List<int> _attributes;
        private VertexDefaultDescription[] _vertexDefaultDescriptions;
        private bool _disposed;

        public VertexArrayObject([DisallowNull] IVertexBufferObject vbo, PrimitiveType primitiveType, VertexDefaultDescription[] vertexDefaultDescriptions = null)
        {
            _primitiveType = primitiveType;
            _drawMethod = DrawMethod.DRAW_ARRAYS;
            _vertexArrayObject = GL.GenVertexArray();
            _attributes = new List<int>();

            if (vertexDefaultDescriptions != null)
                _vertexDefaultDescriptions = vertexDefaultDescriptions;
            else
                _vertexDefaultDescriptions = new VertexDefaultDescription[0];

            GL.BindVertexArray(_vertexArrayObject);
            BindVertexBuffer(vbo);
            GL.BindVertexArray(0);
            _disposed = false;
        }

        public VertexArrayObject([DisallowNull] IVertexBufferObject vbo, [DisallowNull] IElementBufferObject ebo, PrimitiveType primitiveType, VertexDefaultDescription[] vertexDefaultDescriptions = null)
        {
            _primitiveType = primitiveType;
            _drawElementType = ebo.DrawElementsType;
            _drawMethod = DrawMethod.DRAW_ELEMENTS;
            _vertexArrayObject = GL.GenVertexArray();
            _attributes = new List<int>();

            if (vertexDefaultDescriptions != null)
                _vertexDefaultDescriptions = vertexDefaultDescriptions;
            else
                _vertexDefaultDescriptions = new VertexDefaultDescription[0];

            GL.BindVertexArray(_vertexArrayObject);
            ebo.Bind();
            BindVertexBuffer(vbo);
            GL.BindVertexArray(0);

            _disposed = false;
        }

        public VertexArrayObject([DisallowNull] IVertexBufferObject vbo, [DisallowNull] IVertexBufferObject vboInstance, PrimitiveType primitiveType, VertexDefaultDescription[] vertexDefaultDescriptions = null)
        {
            _primitiveType = primitiveType;
            _drawMethod = DrawMethod.DRAW_ARRAYS_INSTANCED;
            _vertexArrayObject = GL.GenVertexArray();
            _attributes = new List<int>();

            if (vertexDefaultDescriptions != null)
                _vertexDefaultDescriptions = vertexDefaultDescriptions;
            else
                _vertexDefaultDescriptions = new VertexDefaultDescription[0];

            GL.BindVertexArray(_vertexArrayObject);
            BindVertexBuffer(vbo);
            BindVertexBuffer(vboInstance, true);
            GL.BindVertexArray(0);

            _disposed = false;
        }

        public VertexArrayObject([DisallowNull] IVertexBufferObject vbo, [DisallowNull] IElementBufferObject ebo, [DisallowNull] IVertexBufferObject vboInstance, PrimitiveType primitiveType, VertexDefaultDescription[] vertexDefaultDescriptions = null)
        {
            _primitiveType = primitiveType;
            _drawElementType = ebo.DrawElementsType;
            _drawMethod = DrawMethod.DRAW_ELEMENTS_INSTANCED;
            _vertexArrayObject = GL.GenVertexArray();
            _attributes = new List<int>();

            if (vertexDefaultDescriptions != null)
                _vertexDefaultDescriptions = vertexDefaultDescriptions;
            else
                _vertexDefaultDescriptions = new VertexDefaultDescription[0];

            GL.BindVertexArray(_vertexArrayObject);
            ebo.Bind();
            BindVertexBuffer(vbo);
            BindVertexBuffer(vboInstance, true);
            GL.BindVertexArray(0);

            _disposed = false;
        }

        private void BindVertexBuffer(IVertexBufferObject vbo, bool isInstancing = false)
        {
            vbo.Bind();
            int attributeOffset = 0;
            foreach (var desc in vbo.VertexDescription)
            {
                switch (desc.Type)
                {
                    case VertexComponentTypes.FLOAT:
                        GL.VertexAttribPointer(desc.LayoutBinding, 1, VertexAttribPointerType.Float, false, vbo.VertexSizeInBytes, attributeOffset);

                        if (isInstancing)
                            GL.VertexAttribDivisor(desc.LayoutBinding, 1);

                        attributeOffset += sizeof(float);
                        _attributes.Add(desc.LayoutBinding);
                        break;

                    case VertexComponentTypes.VEC2:
                        GL.VertexAttribPointer(desc.LayoutBinding, 2, VertexAttribPointerType.Float, false, vbo.VertexSizeInBytes, attributeOffset);

                        if (isInstancing)
                            GL.VertexAttribDivisor(desc.LayoutBinding, 1);

                        attributeOffset += sizeof(float) * 2;
                        _attributes.Add(desc.LayoutBinding);
                        break;

                    case VertexComponentTypes.VEC3:
                        GL.VertexAttribPointer(desc.LayoutBinding, 3, VertexAttribPointerType.Float, false, vbo.VertexSizeInBytes, attributeOffset);

                        if (isInstancing)
                            GL.VertexAttribDivisor(desc.LayoutBinding, 1);

                        attributeOffset += sizeof(float) * 3;
                        _attributes.Add(desc.LayoutBinding);
                        break;

                    case VertexComponentTypes.VEC4:
                        GL.VertexAttribPointer(desc.LayoutBinding, 4, VertexAttribPointerType.Float, false, vbo.VertexSizeInBytes, attributeOffset);

                        if (isInstancing)
                            GL.VertexAttribDivisor(desc.LayoutBinding, 1);

                        attributeOffset += sizeof(float) * 4;
                        _attributes.Add(desc.LayoutBinding);
                        break;

                    case VertexComponentTypes.MAT4:
                        for (int i = 0; i < 4; i++)
                        {
                            GL.VertexAttribPointer(desc.LayoutBinding + i, 4, VertexAttribPointerType.Float, false, vbo.VertexSizeInBytes, attributeOffset);

                            if (isInstancing)
                                GL.VertexAttribDivisor(desc.LayoutBinding + i, 1);

                            attributeOffset += sizeof(float) * 4;
                            _attributes.Add(desc.LayoutBinding + i);
                        }
                        break;
                }
            }
        }

        private void SetDefaultVertices()
        {
            foreach (VertexDefaultDescription desc in _vertexDefaultDescriptions)
            {
                switch (desc.Type)
                {
                    case VertexComponentTypes.FLOAT:
                        GL.VertexAttrib1(desc.LayoutBinding, (float)desc.DefaultValue);
                        break;

                    case VertexComponentTypes.VEC2:
                        var vec2 = (Vector2)desc.DefaultValue;
                        GL.VertexAttrib2(desc.LayoutBinding, vec2.X, vec2.Y);
                        break;

                    case VertexComponentTypes.VEC3:
                        var vec3 = (Vector3)desc.DefaultValue;
                        GL.VertexAttrib3(desc.LayoutBinding, vec3.X, vec3.Y, vec3.Z);
                        break;

                    case VertexComponentTypes.VEC4:
                        var vec4 = (Vector4)desc.DefaultValue;
                        GL.VertexAttrib4(desc.LayoutBinding, vec4.X, vec4.Y, vec4.Z, vec4.W);
                        break;

                    case VertexComponentTypes.MAT4:
                        for (int i = 0; i < 4; i++)
                        {
                            var mat = (Matrix4)desc.DefaultValue;
                            GL.VertexAttrib4(desc.LayoutBinding + i, mat[0, i], mat[1, i], mat[2, i], mat[3, i]);
                        }
                        break;
                }
            }
        }

        public void PreDraw()
        {
            GL.BindVertexArray(_vertexArrayObject);

            foreach (int i in _attributes)
                GL.EnableVertexAttribArray(i);

            SetDefaultVertices();
        }

        public void Draw(int numOfElements, int numOfInstances = 1)
        {
            PreDraw();

            switch (_drawMethod)
            {
                case DrawMethod.DRAW_ARRAYS:
                    GL.DrawArrays(_primitiveType, 0, numOfElements);
                    break;
                case DrawMethod.DRAW_ELEMENTS:
                    GL.DrawElements(_primitiveType, numOfElements, _drawElementType, nint.Zero);
                    break;
                case DrawMethod.DRAW_ARRAYS_INSTANCED:
                    GL.DrawArraysInstanced(_primitiveType, 0, numOfElements, numOfInstances);
                    break;
                case DrawMethod.DRAW_ELEMENTS_INSTANCED:
                    GL.DrawElementsInstanced(_primitiveType, numOfElements, _drawElementType, nint.Zero, numOfInstances);
                    break;
            }

            PostDraw();
        }

        public void PostDraw()
        {
            foreach (int i in _attributes)
                GL.DisableVertexAttribArray(i);

            GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            if (_disposed) return;

            GL.DeleteVertexArray(_vertexArrayObject);
            _vertexArrayObject = -1;
            _disposed = true;
        }
    }
}
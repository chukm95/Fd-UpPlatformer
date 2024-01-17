using FuckedUpPlatformer.Resources.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;

namespace FuckedUpPlatformer
{
    internal class Core
    {

        private byte[] _indices = new byte[]
        {
            0, 1, 2, 0, 2, 3
        };

        private Vector3[] _vertices = new Vector3[]
        {
            new Vector3(-32, 32, 0),
            new Vector3(32, 32, 0),
            new Vector3(32, -32, 0),
            new Vector3(-32, -32, 0)
        };

        private NativeWindow _nativeWindow;

        private Shader _basicBasicShader;
        private ShaderUniform _su_projectionMatrix;
        private ShaderUniform _su_transformMatrix;

        private int _vertexArrayObject;
        private int _indexBuffer;
        private int _vertexBuffer;

        private bool _isRunning;

        public Core()
        {
            //ugly ass code
            Initialize();
            GameLoop();
            Deinitialize();
        }

        private void Initialize()
        {
            NativeWindowSettings nws = new NativeWindowSettings
            {
                API = ContextAPI.OpenGL,
                APIVersion = new Version(4, 5),
                AutoLoadBindings = true,
                IsEventDriven = false,
                ClientSize = new Vector2i(1920, 1080),
                Title = "Fucked up platformer",
                StartVisible = true,
                WindowBorder = WindowBorder.Fixed
            };
            _nativeWindow = new NativeWindow(nws);
            _nativeWindow.Closing += (e) =>
            {
                e.Cancel = true;
                _isRunning = false;
            };

            _basicBasicShader = new Shader("Assets\\Shaders\\BasicBasicShader\\BasicBasicShader.vrt", "\\Assets\\Shaders\\BasicBasicShader\\BasicBasicShader.frt");
            _su_projectionMatrix = _basicBasicShader["projectionMatrix"];
            _su_transformMatrix = _basicBasicShader["transformMatrix"];

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _indexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indexBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length, _indices, BufferUsageHint.StaticDraw);

            _vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector3.SizeInBytes * _vertices.Length, _vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);

            GL.BindVertexArray(0);
            _isRunning = true;
        }

        private void GameLoop()
        {
            GL.Viewport(0, 0, 1920, 1080);
            GL.ClearColor(Color4.Gray);
            while (_isRunning)
            {
                NativeWindow.ProcessWindowEvents(false);

                GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
                _basicBasicShader.Bind();
                _su_projectionMatrix.SetUniform(Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(72f), 1920f / 1080f, 1, 10000));
                _su_transformMatrix.SetUniform(Matrix4.CreateTranslation(new Vector3(0, 0, -2000)));

                GL.BindVertexArray(_vertexArrayObject);
                GL.EnableVertexAttribArray(0);
                GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedByte, IntPtr.Zero);
                GL.DisableVertexAttribArray(0);
                GL.BindVertexArray(0);

                _nativeWindow.Context.SwapBuffers();
            }
        }

        private void Deinitialize()
        {
            _nativeWindow.Close();
            _nativeWindow.Dispose();
        }
    }
}

using FuckedUpPlatformer.Graphics.Buffers;
using FuckedUpPlatformer.Graphics.Vertices;
using FuckedUpPlatformer.Resources.Shaders;
using FuckedUpPlatformer.Util;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Runtime.InteropServices;

namespace FuckedUpPlatformer
{
    internal class Core
    {
        private static Core _instance;

        public static void Run()
        {
            if(_instance == null)
            {
                _instance = new Core();
                _instance.Initialize();
                _instance.GameLoop();
                _instance.Deinitialize();
            }
        }

        public static void Stop()
        {
            if( _instance != null )
            {
                _instance._isRunning = false;
            }
        }

        private NativeWindow _nativeWindow;
        private Window _window;
        private ShaderManager _shaderManager;
        private bool _isRunning;

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

        private VertexPositionColor[] _vertexPositionColors = new VertexPositionColor[]
        {
            new VertexPositionColor{ Position = new Vector3(-32, 32, 0), Color = (Vector4)Color4.Red},
            new VertexPositionColor{ Position = new Vector3(32, 32, 0), Color = (Vector4)Color4.Blue},
            new VertexPositionColor{ Position = new Vector3(32, -32, 0), Color = (Vector4)Color4.Green},
            new VertexPositionColor{ Position = new Vector3(-32, -32, 0), Color = (Vector4)Color4.Purple}
        };

        private Shader _basicBasicShader;
        private ShaderUniform _su_projectionMatrix;
        private ShaderUniform _su_transformMatrix;

        private int _vertexArrayObject;
        private int _indexBuffer;
        private int _vertexBuffer;

        private GraphicsBuffer<VertexPositionColor, byte> _graphicsBuffer;

        private Core()
        {

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

            _window = new Window(_nativeWindow);
            _window.OnCloseWindowRequest += () => { _isRunning = false; };
            _shaderManager = new ShaderManager();

            _isRunning = true;

            _graphicsBuffer = new GraphicsBuffer<VertexPositionColor, byte>(PrimitiveType.Triangles, VertexPositionColor.Description, BufferUsageHint.StaticDraw, Marshal.SizeOf<VertexPositionColor>());
            _graphicsBuffer.BufferVertices(_vertexPositionColors);
            _graphicsBuffer.BufferIndices(_indices);

            _basicBasicShader = _shaderManager.LoadShader("Assets\\Shaders\\BasicBasicShader.fsf");
            _su_projectionMatrix = _basicBasicShader["projectionMatrix"];
            _su_transformMatrix = _basicBasicShader["transformMatrix"];

            //_vertexArrayObject = GL.GenVertexArray();
            //GL.BindVertexArray(_vertexArrayObject);

            //_indexBuffer = GL.GenBuffer();
            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indexBuffer);
            //GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length, _indices, BufferUsageHint.StaticDraw);

            //_vertexBuffer = GL.GenBuffer();
            //GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            //GL.BufferData(BufferTarget.ArrayBuffer, Vector3.SizeInBytes * _vertices.Length, _vertices, BufferUsageHint.StaticDraw);
            //GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);

            //GL.BindVertexArray(0);
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
                _su_transformMatrix.SetUniform(Matrix4.CreateTranslation(new Vector3(0, 0, -1000)));

                //GL.BindVertexArray(_vertexArrayObject);
                //GL.EnableVertexAttribArray(0);
                //GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedByte, IntPtr.Zero);
                //GL.DisableVertexAttribArray(0);
                //GL.BindVertexArray(0);

                _graphicsBuffer.Draw(_indices.Length);

                _nativeWindow.Context.SwapBuffers();
            }
        }

        private void Deinitialize()
        {
            _graphicsBuffer.Dispose();
            _shaderManager.Dispose();
            _nativeWindow.Close();
            _nativeWindow.Dispose();
        }
    }
}

using FuckedUpPlatformer.Graphics;
using FuckedUpPlatformer.Graphics.Batching;
using FuckedUpPlatformer.Graphics.Buffers;
using FuckedUpPlatformer.Graphics.Cameras;
using FuckedUpPlatformer.Graphics.Vertices;
using FuckedUpPlatformer.Resources.Shaders;
using FuckedUpPlatformer.Util;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace FuckedUpPlatformer
{
    internal class Core {

        public static Window Window => _instance._window;
        public static GameTime GameTime => _instance._gameTime;

        private static Core _instance;

        private NativeWindow _nativeWindow;
        private Window _window;
        private ShaderManager _shaderManager;
        private GameTime _gameTime;
        private bool _isRunning;

        //private Shader _basicBasicShader;
        //private ShaderUniform _su_projectionMatrix;
        //private ShaderUniform _su_transformMatrix;

        //private QuadBatcher _quadBatcher;


        //Constructor to have for singleton use
        private Core() {

        }

        //Starts program
        public static void Run() {
            if (_instance == null) {
                _instance = new Core();
                _instance.Initialize();
                _instance.GameLoop();
                _instance.Deinitialize();
            }
        }

        //shuts program down correctly
        public static void Stop() {
            if (_instance != null) {
                _instance._isRunning = false;
            }
        }

        //private struct rectPos {
        //    public float rotation;
        //    public Vector3 scale;
        //    public Vector3 position;
        //    public Color4 color;
        //}

        //public Color4[] colors = new Color4[]{
        //    Color4.Red,
        //    Color4.Blue,
        //    Color4.Green,
        //    Color4.Orange
        //};

        //private rectPos[] positions;

        //Creates basic components for program
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

            _window = new Window(_nativeWindow);
            _window.OnCloseWindowRequest += () => { Core.Stop(); };
            _shaderManager = new ShaderManager();
            _gameTime = new GameTime();
            _isRunning = true;

            //_basicBasicShader = _shaderManager.LoadShader("Assets\\Shaders\\BasicBasicShader.fsf");
            //_su_projectionMatrix = _basicBasicShader["projectionMatrix"];
            //_su_transformMatrix = _basicBasicShader["transformMatrix"];

            //Random r = new Random();
            //Vector3 offset = new Vector3(960, 540, 0);
            //_quadBatcher = new QuadBatcher(20, BufferUsageHint.DynamicDraw);

            //positions = new rectPos[200];

            //for (int i = 0; i < 200; i++)
            //{
            //    positions[i] = new rectPos {
            //        rotation = MathHelper.DegreesToRadians(r.Next(359)),
            //        scale = new Vector3((float)(r.NextDouble()) + 0.5f),
            //        position = new Vector3(r.Next(1920), r.Next(1080), 0) - offset,
            //        color = colors[r.Next(4)]
            //    };
            //}

            //_quadBatcher.Begin();
            //for (int i = 0; i < 1; i++)
            //{
            //    _quadBatcher.Batch(new Vector3(r.Next(1280), r.Next(720), 0) - offset, Vector3.One, Vector3.Zero, Color4.White);
            //}
            //_quadBatcher.End();
        }

        //the standard gameloop of updating shaders and objects

        float rot = 0f;
        private void GameLoop()
        {
            GL.Viewport(0, 0, 1920, 1080);
            GL.ClearColor(Color4.Gray);

            Camera c = new OrthographicCamera(Vector3.Zero, MathUtil.ToRadians(new Vector3(0, 0, 45)), 1920, 1080, 1f, 2000f);
            CameraController cc = new CameraController(_nativeWindow, c);

            while (_isRunning)
            {
                _nativeWindow.NewInputFrame();
                NativeWindow.ProcessWindowEvents(false);
                _gameTime.Update();

                //c.Update();
                //cc.Update();

                //GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
                //_basicBasicShader.Bind();
                //_su_projectionMatrix.SetUniform(c.ViewProjectionMatrix);
                //_su_transformMatrix.SetUniform(Matrix4.CreateTranslation(new Vector3(0, 0, -1000)));

                //_quadBatcher.Begin();
                //for (int i = 0; i < 199; i++)
                //{
                //    rectPos r = positions[i];
                //    _quadBatcher.Batch(r.position, r.scale, new Vector3(0, 0, r.rotation), r.color);
                //}
                //rectPos x = positions[199];
                //    _quadBatcher.Batch(x.position, x.scale, new Vector3(0, 0, rot), x.color);
                //_quadBatcher.End();

                //_quadBatcher.Draw();

                //rot += MathHelper.DegreesToRadians(45f * _gameTime.Delta);

                _nativeWindow.Context.SwapBuffers();
            }
        }


        private void Deinitialize()
        {
            _shaderManager.Dispose();
            _nativeWindow.Close();
            _nativeWindow.Dispose();
        }
    }
}

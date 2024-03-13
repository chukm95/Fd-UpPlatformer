using FuckedUpPlatformer.GameStateManagement;
using FuckedUpPlatformer.GameStates;
using FuckedUpPlatformer.Resources.Shaders;
using FuckedUpPlatformer.Resources.Textures;
using FuckedUpPlatformer.Util;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Threading;


namespace FuckedUpPlatformer {
    internal class Core {

        public static Window Window => _instance._window;
        public static ShaderManager ShaderManager => _instance._shaderManager;
        public static TextureManager TextureManager => _instance._textureManager;
        public static GameTime GameTime => _instance._gameTime;
        public static GameStateManager GameStateManager => _instance._gameStateManager;

        private static Core _instance;

        private NativeWindow _nativeWindow;
        private Window _window;
        private ShaderManager _shaderManager;
        private TextureManager _textureManager;
        private GameTime _gameTime;
        private GameStateManager _gameStateManager;
        private bool _isRunning;

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

        //Creates basic components for program
        private void Initialize() {
            NativeWindowSettings nws = new NativeWindowSettings {
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
            _shaderManager = new ShaderManager("FuckedUpPlatformer.Assets.Shaders.");
            _textureManager = new TextureManager("FuckedUpPlatformer.Assets.Textures.");
            _gameTime = new GameTime();
            _gameStateManager = new GameStateManager();
            _isRunning = true;

            _gameStateManager.Add(new GS_Test());
            _gameStateManager.Set(GameStateIdentifierz.TEST1);
        }

        //the standard gameloop of updating shaders and objects
        private void GameLoop() {
            GL.Viewport(0, 0, 1920, 1080);
            GL.ClearColor(Color4.Gray);

            while (_isRunning) {
                _nativeWindow.NewInputFrame();
                NativeWindow.ProcessWindowEvents(false);
                _gameTime.Update();
                _gameStateManager.Update();

                _nativeWindow.Context.SwapBuffers();
            }
        }


        private void Deinitialize() {
            _gameStateManager.Dispose();
            _shaderManager.Dispose();
            _nativeWindow.Close();
            _nativeWindow.Dispose();
        }
    }
}

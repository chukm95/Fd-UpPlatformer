using FuckedUpPlatformer.GameStateManagement;
using FuckedUpPlatformer.Graphics.Cameras;
using FuckedUpPlatformer.Resources.Shaders;
using FuckedUpPlatformer.Resources.Textures;
using FuckedUpPlatformer.Graphics.Batching;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;

namespace FuckedUpPlatformer.GameStates {
    internal class TestState : IGameState {

        private struct rectPos
        {
            public float rotation;
            public Vector3 scale;
            public Vector3 position;
            public Color4 color;
        }

        public Enum GameStateId => GameStateIdentifierz.TEST1;

        private Camera _camera;

        private Shader _basicBasicShader;
        private ShaderUniform _su_projectionMatrix;
        private ShaderUniform _su_transformMatrix;

        private Texture _texture;

        private QuadBatcher _quadBatcher;

        private rectPos[] positions;

        public TestState() {
        }

        public void Activate() {
            _camera = new OrthographicCamera(Vector3.Zero, Vector3.Zero, 1920, 1080, 0.1f, 20000);

            _basicBasicShader = Core.ShaderManager.LoadShader("Assets\\Shaders\\BasicBasicShader.fsf");
            _su_projectionMatrix = _basicBasicShader["projectionMatrix"];
            _su_transformMatrix = _basicBasicShader["transformMatrix"];

            _texture = new Texture("Assets\\Textures\\TestTexture2.png");

            _quadBatcher = new QuadBatcher(20, BufferUsageHint.DynamicDraw);

            positions = new rectPos[200];

            Random r = new Random();
            Vector3 offset = new Vector3(640, 360, 1000);

            for (int i = 0; i < 200; i++) {
                positions[i] = new rectPos
                {
                    rotation = MathHelper.DegreesToRadians(r.Next(359)),
                    scale = new Vector3((float)(r.NextDouble()) + 0.5f),
                    position = new Vector3(r.Next(1280), r.Next(720), 0) - offset,
                    color = Color4.White
                };
            }
        }

        public void Update() {
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            _basicBasicShader.Bind();
            _texture.Bind(0);
            _su_projectionMatrix.SetUniform(Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(72f), 1920f / 1080f, 1, 10000));
            _su_transformMatrix.SetUniform(Matrix4.CreateTranslation(new Vector3(0, 0, -1000)));

            _quadBatcher.Begin();
            for (int i = 0; i < 200; i++) {
                rectPos r = positions[i];
                _quadBatcher.Batch(r.position, r.scale, new Vector3(0, 0, r.rotation), r.color);
            }
            _quadBatcher.End();
            _quadBatcher.Draw();
        }

        public void Deactivate() {
        }

        public void Dispose() {
        }

    }
}
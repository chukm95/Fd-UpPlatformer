using FuckedUpPlatformer.GameStateManagement;
using FuckedUpPlatformer.Graphics.Cameras;
using FuckedUpPlatformer.Resources.Shaders;
using FuckedUpPlatformer.Resources.Textures;
using FuckedUpPlatformer.Graphics.Batching;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;

namespace FuckedUpPlatformer.GameStates {
    internal class TestState2 : IGameState {
        public Enum GameStateId => GameStateIdentifierz.TEST2;

        private Camera _camera;

        private Shader _basicBasicShader;
        private ShaderUniform _su_projectionMatrix;
        private ShaderUniform _su_transformMatrix;
        private ShaderUniform _su_Sampler;

        private Texture _texture;
        private TextureAtlas _textureAtlas;
        private SpriteAnimation _spriteAnimation;

        private QuadBatcher _quadBatcher;

        public TestState2() {
        }

        public void Activate() {
            _camera = new OrthographicCamera(Vector3.Zero, Vector3.Zero, 1920, 1080, 0.1f, 20000);

            _basicBasicShader = Core.ShaderManager.LoadShader("Assets\\Shaders\\BasicTextureShader.fsf");
            _su_projectionMatrix = _basicBasicShader["projectionMatrix"];
            _su_transformMatrix = _basicBasicShader["transformMatrix"];
            _su_Sampler = _basicBasicShader["textureSampler"];

            _texture = new Texture("Assets\\Textures\\TestFrameTexture.png");
            _textureAtlas = new TextureAtlas(2, 2);
            _spriteAnimation = new SpriteAnimation(_textureAtlas, new[]
            {
                (TimeSpan.FromSeconds(1), 0),
                (TimeSpan.FromSeconds(2), 1),
                (TimeSpan.FromSeconds(1), 2),
                (TimeSpan.FromSeconds(4), 3)
            });

            _quadBatcher = new QuadBatcher(20, BufferUsageHint.DynamicDraw);
        }

        public void Update() {

            _spriteAnimation.Update();

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            _basicBasicShader.Bind();
            _texture.Bind(0);

            _su_projectionMatrix.SetUniform(Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(72f), 1920f / 1080f, 0.1f, 10000));
            _su_transformMatrix.SetUniform(Matrix4.CreateTranslation(new Vector3(0, 0, -1000)));
            _su_Sampler.SetUniform(0);

            _quadBatcher.Begin();
            //_quadBatcher.Batch(new Vector3(-128, 128, -1000), new Vector3(2, 2, 1), Vector3.Zero, Color4.White, _textureAtlas[0]);
            //_quadBatcher.Batch(new Vector3(128, 128, -1000), new Vector3(2, 2, 1), Vector3.Zero, Color4.White, _textureAtlas[1]);
            //_quadBatcher.Batch(new Vector3(-128, -128, -1000), new Vector3(2, 2, 1), Vector3.Zero, Color4.White, _textureAtlas[2]);
            //_quadBatcher.Batch(new Vector3(128, -128, -1000), new Vector3(2, 2, 1), Vector3.Zero, Color4.White, _textureAtlas[3]);

            _quadBatcher.Batch(new Vector3(-128, 128, -1000), new Vector3(2, 2, 1), Vector3.Zero, Color4.White, _spriteAnimation.CurrentFrame);
            _quadBatcher.End();
            _quadBatcher.Draw();
        }

        public void Deactivate() {
        }

        public void Dispose() {
        }

    }
}
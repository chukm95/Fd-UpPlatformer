using FuckedUpPlatformer.GameStateManagement;
using FuckedUpPlatformer.Graphics.Batching;
using FuckedUpPlatformer.Graphics.Cameras;
using FuckedUpPlatformer.Graphics.Shapes;
using FuckedUpPlatformer.Resources.Shaders;
using FuckedUpPlatformer.Resources.Textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;

namespace FuckedUpPlatformer.GameStates
{
    internal class GS_Test : IGameState
    {
        public Enum GameStateId => GameStateIdentifierz.TEST1;

        private readonly int[] defaultSamplers = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

        private Camera _camera;

        private Shader _basicBasicShader;
        private ShaderUniform _su_projectionMatrix;
        private ShaderUniform _su_transformMatrix;
        private ShaderUniform _su_Sampler;

        private Texture _texture1;
        private Texture _texture2;

        private TextureAtlas _textureAtlas;
        private SpriteAnimation _spriteAnimation;

        private SpriteBatcher _spriteBatcher;
        private Shape _circleShape;

        public GS_Test() {

        }

        public void Activate() {
            _camera = new OrthographicCamera(Vector3.Zero, Vector3.Zero, 1920, 1080, 0.1f, 20000);

            _basicBasicShader = Core.ShaderManager.LoadShader("DefaultShader.fsf");
            _su_projectionMatrix = _basicBasicShader["projectionMatrix"];
            _su_transformMatrix = _basicBasicShader["transformMatrix"];
            _su_Sampler = _basicBasicShader["textureSampler"];

            _texture1 = Core.TextureManager.LoadTexture("TestFrameTexture.png");
            _texture2 = Core.TextureManager.LoadTexture("Test_Texture.png");
            _textureAtlas = new TextureAtlas(2, 2);
            _spriteAnimation = new SpriteAnimation(_textureAtlas, new[]
            {
                            (TimeSpan.FromSeconds(1), 0),
                            (TimeSpan.FromSeconds(2), 1),
                            (TimeSpan.FromSeconds(1), 2),
                            (TimeSpan.FromSeconds(4), 3)
                        });

            _spriteBatcher = new SpriteBatcher(20, BufferUsageHint.DynamicDraw);
            _circleShape = ShapeGenerator.GenerateCenteredCircle(32);
        }

        public void Update() {
            _spriteAnimation.Update();

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            _basicBasicShader.Bind();

            _su_projectionMatrix.SetUniform(Matrix4.CreateOrthographic(1920, 1080, 0.1f, 10000));
            _su_transformMatrix.SetUniform(Matrix4.CreateTranslation(new Vector3(0, 0, -10)));
            _su_Sampler.SetUniform(defaultSamplers);

            _spriteBatcher.Begin();

            _spriteBatcher.Batch(new Vector3(-128, 128, 0), Color4.White, new Vector3(_texture1.Width / 2f, _texture1.Height / 2f, 1), Vector3.Zero, Vector3.Zero, _texture1, _spriteAnimation.CurrentFrame);
            _spriteBatcher.Batch(new Vector3(256, 256, 0), Color4.Red, new Vector3(128, 128, 1), Vector3.Zero, Vector3.Zero);
            _spriteBatcher.Batch(_circleShape, new Vector3(128, -256, 0), Color4.White, new Vector3(200, 200, 1), Vector3.Zero, Vector3.Zero, _texture1, _spriteAnimation.CurrentFrame);
            _spriteBatcher.Batch(new Vector3(-128, -128, 0), Color4.White, new Vector3(_texture1.Width / 2f, _texture1.Height / 2f, 1), Vector3.Zero, Vector3.Zero, _texture2);

            _spriteBatcher.End();
            _spriteBatcher.Draw();
        }

        public void Deactivate() {
            _spriteBatcher.Dispose();
        }

        public void Dispose() {
        }
    }
}

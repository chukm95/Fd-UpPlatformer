using FuckedUpPlatformer.Graphics.Cameras;
using FuckedUpPlatformer.Util;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace FuckedUpPlatformer.Graphics
{
    internal class CameraController
    {
        private const float CAMERA_SPEED = 128f;
        private const float CAMERA_SPEED_MULTIPLIER = 2f;

        private readonly GameTime _gameTime;
        private Camera _camera;
        private NativeWindow _nativeWindow;

        public CameraController(NativeWindow nativeWindow, Camera camera)
        {
            _gameTime = Core.GameTime;
            _camera = camera;
            _nativeWindow = nativeWindow;
        }

        public void Update()
        {
            Vector3 cameraPos = _camera.Position;
            Vector3 cameraVelocity = Vector3.Zero;

            if(_nativeWindow.IsKeyDown(Keys.A) && !_nativeWindow.IsKeyDown(Keys.D))
            {
                cameraVelocity.X += CAMERA_SPEED * _gameTime.Delta; 
            }
            else if(_nativeWindow.IsKeyDown(Keys.D) && !_nativeWindow.IsKeyDown(Keys.A))
            {
                cameraVelocity.X -= CAMERA_SPEED * _gameTime.Delta;
            }

            if (_nativeWindow.IsKeyDown(Keys.LeftShift))
                cameraVelocity *= CAMERA_SPEED_MULTIPLIER;

            _camera.Position = cameraPos + cameraVelocity;
        }
    }
}

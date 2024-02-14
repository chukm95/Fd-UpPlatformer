using OpenTK.Mathematics;

namespace FuckedUpPlatformer.Graphics.Cameras
{
    public class PerspectiveCamera : Camera
    {
        public float FOV
        {
            get => _fov;
            set
            {
                _fov = value;
                ProjectionChanged();
            }
        }

        private float _fov;

        public PerspectiveCamera(Vector3 position, Vector3 rotation, float fov, float zNear, float zFar) : base(position, rotation, zNear, zFar)
        {
            _fov = fov;
        }

        protected override Matrix4 CalculateProjection()
        {
            return Matrix4.CreatePerspectiveFieldOfView(_fov, Core.Window.AspectRatio, ZNear, ZFar);
        }
    }
}
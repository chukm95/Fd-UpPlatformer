using OpenTK.Mathematics;

namespace FuckedUpPlatformer.Graphics.Cameras
{
    public class OrthographicCamera : Camera
    {

        public float Width {
            get => _width;
            set { 
                _width = value;
                ProjectionChanged();
            }
        }
        public float Height
        {
            get => _height;
            set
            {
                _height = value;
                ProjectionChanged();
            }
        }

        private float _width;
        private float _height;

        public OrthographicCamera(Vector3 position, Vector3 rotation, float width, float height, float zNear, float zFar) : base(position, rotation, zNear, zFar)
        {
            _width = width;
            _height = height;
        }

        protected override Matrix4 CalculateProjection()
        {
            return Matrix4.CreateOrthographic(_width, _height, ZNear, ZFar);
        }

        protected override Matrix4 CalculateView()
        {
            return Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(Rotation)) * Matrix4.CreateTranslation(Position); 
        }
    }
}
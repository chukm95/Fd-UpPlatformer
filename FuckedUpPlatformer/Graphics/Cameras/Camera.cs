using OpenTK.Mathematics;

namespace FuckedUpPlatformer.Graphics.Cameras
{
    public abstract class Camera
    {
        public Vector3 Position
        {
            get => _position;
            set
            {
                _position = value;
                _isViewChanged = true;
            }
        }

        public float Position_X
        {
            get => _position.X;
            set
            {
                _position.X = value;
                _isViewChanged = true;
            }
        }

        public float Position_Y
        {
            get => _position.Y;
            set
            {
                _position.Y = value;
                _isViewChanged = true;
            }
        }

        public float Position_Z
        {
            get => _position.Y;
            set
            {
                _position.Z = value;
                _isViewChanged = true;
            }
        }

        public Vector3 Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                _isViewChanged = true;
            }
        }

        public float Rotation_X {
            get => _rotation.X;
            set {
                _rotation.X = value;
                _isViewChanged = true;
            }
        }
        public float Rotation_Y {
            get => _rotation.Y;
            set {
                _rotation.Y = value;
                _isViewChanged = true;
            }
        }
        public float Rotation_Z {
            get => _rotation.Z;
            set {
                _rotation.Z = value;
                _isViewChanged = true;
            }
        }

        public float ZNear
        {
            get => _zNear;
            set
            {
                _zNear = value;
                _isProjectionChanged = true;
            }
        }

        public float ZFar {
            get => _zFar;
            set {
                _zFar = value;
                _isProjectionChanged = true;
            }
        }

        public Matrix4 ProjectionMatrix => _projectionMatrix;
        public Matrix4 ViewMatrix => _viewMatrix;
        public Matrix4 ViewProjectionMatrix => _viewProjectionMatrix;

        private Vector3 _position;
        private Vector3 _rotation;
        private float _zNear; 
        private float _zFar;

        private Matrix4 _projectionMatrix;
        private bool _isProjectionChanged;
        private Matrix4 _viewMatrix;
        private bool _isViewChanged;
        private Matrix4 _viewProjectionMatrix;

        protected Camera(Vector3 position, Vector3 rotation, float zNear, float zFar)
        {
            _position = position;
            _rotation = rotation; 
            _zNear = zNear;
            _zFar = zFar;
            _isProjectionChanged = true;
            _isViewChanged = true;
        }

        public void Update()
        {
            if( _isProjectionChanged || _isViewChanged )
            {
                if (_isProjectionChanged)
                    _projectionMatrix =  CalculateProjection();

                if (_isViewChanged)
                    _viewMatrix =  CalculateView();

                _viewProjectionMatrix = _viewMatrix * _projectionMatrix;
                _isProjectionChanged = false;
                _isViewChanged = false;
            }
        }

        protected void ProjectionChanged()
        {
            _isProjectionChanged = true;
        }

        protected abstract Matrix4 CalculateProjection();

        protected virtual Matrix4 CalculateView()
        {
            var rotationQuat = Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(_rotation));
            return Matrix4.LookAt(_position, _position + Vector3.TransformPosition(Vector3.UnitZ, rotationQuat), Vector3.UnitY);
        }
    }
}
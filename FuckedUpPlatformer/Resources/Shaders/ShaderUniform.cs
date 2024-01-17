using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace FuckedUpPlatformer.Resources.Shaders
{
    internal class ShaderUniform
    {
        public ActiveUniformType UniformType => _type;

        private readonly int _location;
        private readonly string _name;
        private readonly ActiveUniformType _type;

        internal ShaderUniform(int location, string name, ActiveUniformType type)
        {
            _location = location;
            _name = name;
            _type = type;
        }
        public void SetUniform(bool value)
        {
            if (_type == ActiveUniformType.Bool)
            {
                GL.Uniform1(_location, value ? 1 : 0);
            }
        }

        public void SetUniform(int value)
        {
            if (_type == ActiveUniformType.Int || _type == ActiveUniformType.Sampler2D)
            {
                GL.Uniform1(_location, value);
            }
        }

        public void SetUniform(int[] values)
        {
            if (_type == ActiveUniformType.Int || _type == ActiveUniformType.Sampler2D)
            {
                GL.Uniform1(_location, values.Length, values);
            }
        }

        public void SetUniform(float value)
        {
            if (_type == ActiveUniformType.Float)
            {
                GL.Uniform1(_location, value);
            }
        }

        public void SetUniform(double value)
        {
            if (_type == ActiveUniformType.Double)
            {
                GL.Uniform1(_location, value);
            }
        }

        public void SetUniform(Vector2 value)
        {
            if (_type == ActiveUniformType.FloatVec2)
            {
                GL.Uniform2(_location, value);
            }
        }

        public void SetUniform(Vector3 value)
        {
            if (_type == ActiveUniformType.FloatVec3)
            {
                GL.Uniform3(_location, value);
            }
        }

        public void SetUniform(Vector4 value)
        {
            if (_type == ActiveUniformType.FloatVec4)
            {
                GL.Uniform4(_location, value);
            }
        }

        public void SetUniform(Matrix2 value)
        {
            if (_type == ActiveUniformType.FloatMat2)
            {
                GL.UniformMatrix2(_location, false, ref value);
            }
        }

        public void SetUniform(Matrix3 value)
        {
            if (_type == ActiveUniformType.FloatMat3)
            {
                GL.UniformMatrix3(_location, false, ref value);
            }
        }

        public void SetUniform(Matrix4 value)
        {
            if (_type == ActiveUniformType.FloatMat4)
            {
                GL.UniformMatrix4(_location, false, ref value);
            }
        }
    }
}

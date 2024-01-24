using OpenTK.Compute.OpenCL;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FuckedUpPlatformer.Resources.Shaders
{
    internal class Shader
    {
        private const string VERTEX_SHADER_TAG = "$VERTEX$";
        private const string FRAGMENT_SHADER_TAG = "$FRAGMENT$";

        public string FilePath
        {
            get => _filePath;
        }

        public bool IsDisposed
        {
            get => _isDisposed;
        }

        public ShaderUniform this[string name]
        {
            get
            {
                _uniforms.TryGetValue(name, out var value);
                return value;
            }
        }

        private string _filePath;
        private Dictionary<string, ShaderUniform> _uniforms;
        private int _programId;
        private bool _isDisposed;

        public Shader(string filePath)
        {
            _filePath = filePath;
            _uniforms = new Dictionary<string, ShaderUniform>();
            LoadShaderSource(filePath, out var vertexSource, out var fragmentSource);

            var vertexShaderId = CreateShader(ShaderType.VertexShader, vertexSource);
            var fragmentShaderId = CreateShader(ShaderType.FragmentShader, fragmentSource);
            _programId = CreateProgram(vertexShaderId, fragmentShaderId);
            _isDisposed = false;
        }

        private void LoadShaderSource(string filePath, out string vertexSource, out string fragmentSource)
        {
            StringBuilder vertexSourceBuilder = new StringBuilder();
            StringBuilder fragmentSourceBuilder = new StringBuilder();

            using (StreamReader sr = new StreamReader(File.OpenRead(filePath)))
            {
                StringBuilder currentBuilder = null;
                string line = null;

                while ((line = sr.ReadLine()) != null)
                {
                    switch (line)
                    {
                        case VERTEX_SHADER_TAG:
                            currentBuilder = vertexSourceBuilder;
                            break;
                        case FRAGMENT_SHADER_TAG:
                            currentBuilder = fragmentSourceBuilder;
                            break;
                        default:
                            currentBuilder?.AppendLine(line);
                            break;
                    }
                }
            }

            vertexSource = vertexSourceBuilder.ToString();
            fragmentSource = fragmentSourceBuilder.ToString();
        }     

        private int CreateShader(ShaderType type, string source)
        {
            int shaderId = GL.CreateShader(type);

            if (shaderId == 0)
                throw new Exception($"Failed to create a {type} shader!");

            GL.ShaderSource(shaderId, source);

            GL.CompileShader(shaderId);

            int compileStatus;
            GL.GetShader(shaderId, ShaderParameter.CompileStatus, out compileStatus);

            if (compileStatus == 0)
                throw new Exception($"Failed to compile a {type} shader! \n{GL.GetShaderInfoLog(shaderId)}");

            return shaderId;
        }

        private int CreateProgram(int vertexShaderId, int fragmentShaderId)
        {
            int programId = GL.CreateProgram();

            if (programId == 0)
                throw new Exception($"Failed to create program!");

            GL.AttachShader(programId, vertexShaderId);
            GL.AttachShader(programId, fragmentShaderId);

            GL.LinkProgram(programId);
            int linkStatus;
            GL.GetProgram(programId, GetProgramParameterName.LinkStatus, out linkStatus);

            if (linkStatus == 0)
                throw new Exception("Failed to link!");

            GL.DeleteShader(vertexShaderId);
            GL.DeleteShader(fragmentShaderId);

            GL.ValidateProgram(programId);
            int validationStatus;
            GL.GetProgram(programId, GetProgramParameterName.ValidateStatus, out validationStatus);

            if (validationStatus == 0)
                throw new Exception("Failed to validate!");

            GL.GetProgram(programId, GetProgramParameterName.ActiveUniforms, out var numOfUniforms);
            for (int i = 0; i < numOfUniforms; i++)
            {
                GL.GetActiveUniform(programId, i, 60, out var length, out var size, out var type, out var name);
                int location = GL.GetUniformLocation(programId, name);
                if (type == ActiveUniformType.Sampler2D)
                {
                    if (name.EndsWith("[0]"))
                        name = name.Remove(name.Length - 3, 3);
                }

                _uniforms.Add(name, new ShaderUniform(location, name, type));
            }

            return programId;
        }

        public void Bind()
        {
            if (!_isDisposed){
                GL.UseProgram(_programId);
            }
        }

        public void Dispose()
        {
            if (_isDisposed) return;

            GL.DeleteProgram(_programId);
            _programId = -1;
            _isDisposed = true;
        }
    }
}

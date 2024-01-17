using OpenTK.Compute.OpenCL;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;

namespace FuckedUpPlatformer.Resources.Shaders
{
    internal class Shader
    {
        public ShaderUniform this[string name]
        {
            get
            {
                _uniforms.TryGetValue(name, out var value);
                return value;
            }
        }

        private Dictionary<string, ShaderUniform> _uniforms;
        private int _programId;

        public Shader(string vertexSourceFilePath, string fragmentSourceFilePath)
        {
            _uniforms = new Dictionary<string, ShaderUniform>();
            var vertexSource = LoadShaderSource(vertexSourceFilePath);
            var fragmentSource = LoadShaderSource(fragmentSourceFilePath);

            var vertexShaderId = CreateShader(ShaderType.VertexShader, vertexSource);
            var fragmentShaderId = CreateShader(ShaderType.FragmentShader, fragmentSource);
            _programId = CreateProgram(vertexShaderId, fragmentShaderId);
        }

        private string LoadShaderSource(string filePath)
        {
            var path = string.Join(Path.DirectorySeparatorChar, Directory.GetCurrentDirectory(), filePath);

            if (!File.Exists(path))
                throw new Exception("File doesnt exit!");

            return File.ReadAllText(path);
        }

        private int CreateShader(ShaderType type, string source)
        {
            int shaderId = GL.CreateShader(type);

            if(shaderId == 0) 
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

            if(programId == 0)
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
            GL.UseProgram(_programId);
        }
    }
}

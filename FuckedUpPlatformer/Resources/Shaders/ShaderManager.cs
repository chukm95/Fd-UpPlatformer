using System.Collections.Generic;

namespace FuckedUpPlatformer.Resources.Shaders
{
    internal class ShaderManager {
        private Dictionary<string, Shader> _shaders;

        public ShaderManager() {
            _shaders = new Dictionary<string, Shader>();
        }

        public Shader LoadShader(string filePath) {
            if (_shaders.ContainsKey(filePath)) {
                return _shaders[filePath];
            } else {
                var shader = new Shader(filePath);
                _shaders.Add(filePath, shader);
                return shader;
            }
        }

        public void UnloadShader(string filePath) {
            if (_shaders.Remove(filePath, out var shader)) {
                shader.Dispose();
            }
        }

        public void Dispose() {
            foreach(Shader sh in _shaders.Values){
                sh.Dispose();
            }
        }
    }
}
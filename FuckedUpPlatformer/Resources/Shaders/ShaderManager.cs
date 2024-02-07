using System.Collections.Generic;

namespace FuckedUpPlatformer.Resources.Shaders
{
    //dictionary of shaders <filepath of shader, shader object>
    internal class ShaderManager {
        private Dictionary<string, Shader> _shaders;

        // constructor
        public ShaderManager() {
            _shaders = new Dictionary<string, Shader>();
        }

        //load shader given filepath of shader
        public Shader LoadShader(string filePath) {
            if (_shaders.ContainsKey(filePath)) {
                return _shaders[filePath];
            } else {
                var shader = new Shader(filePath);
                _shaders.Add(filePath, shader);
                return shader;
            }
        }

        //remove shader from dictionary
        public void UnloadShader(string filePath) {
            if (_shaders.Remove(filePath, out var shader)) {
                shader.Dispose();
            }
        }

        //remove all shaders within the dictionary
        public void Dispose() {
            foreach(Shader sh in _shaders.Values){
                sh.Dispose();
            }
        }
    }
}
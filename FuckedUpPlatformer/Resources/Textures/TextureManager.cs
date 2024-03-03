using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuckedUpPlatformer.Resources.Textures
{
    internal class TextureManager
    {
        private Dictionary<string, Texture> _textures;

        internal TextureManager() {
            _textures = new Dictionary<string, Texture>();
        }

        //load texture given filepath of shader
        public Texture LoadTexture(string filePath) {
            if (_textures.ContainsKey(filePath)) {
                return _textures[filePath];
            }
            else {
                var texture = new Texture(filePath);
                _textures.Add(filePath, texture);
                return texture;
            }
        }

        //remove shader from dictionary
        public void UnloadTexture(string filePath) {
            if (_textures.Remove(filePath, out var text)) {
                text.Dispose();
            }
        }

        //remove all shaders within the dictionary
        public void Dispose() {
            foreach (Texture text in _textures.Values) {
                text.Dispose();
            }
        }


    }
}

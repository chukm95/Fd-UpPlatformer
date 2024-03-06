using OpenTK.Mathematics;
using System.Collections.Generic;

namespace FuckedUpPlatformer.Resources.Textures
{
    internal class TextureAtlas
    {
        public Vector4 this[int value] {
            get => _frames[value % _maxNumOfFrames];
        }

        private readonly int _framesHor;
        private readonly int _framesVer;
        private readonly int _maxNumOfFrames;

        private Dictionary<int, Vector4> _frames;

        public TextureAtlas(int framesHor, int framesVer) {

            _frames = new Dictionary<int, Vector4>();
            _maxNumOfFrames = framesVer * framesVer;
            float texCoordWidth = 1f / (float)framesHor;
            float texCoordHeight= 1f / (float)framesVer;

            int index = 0;

            for (int y = 0; y < framesVer; y++) {
                for (int x = 0; x < framesHor; x++) {
                    _frames.Add(index++, new Vector4(x * texCoordWidth, y * texCoordHeight, texCoordWidth, texCoordHeight));
                }
            }
        }
    }
}
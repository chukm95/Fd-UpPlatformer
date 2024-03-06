using FuckedUpPlatformer.Util;
using OpenTK.Mathematics;
using System;

namespace FuckedUpPlatformer.Resources.Textures
{
    internal class SpriteAnimation
    {
        public Vector4 CurrentFrame => _atlas[_frames[_index].Item2];

        private readonly GameTime _gameTime;
        private readonly TextureAtlas _atlas;
        private readonly (TimeSpan, int)[] _frames;
        private TimeSpan _elapsedTime;
        private int _index;

        private TimeSpan _timeTarget;
        private int _currentFrame;

        public SpriteAnimation(TextureAtlas atlas, params (TimeSpan, int)[] frames) 
        {
            _gameTime = Core.GameTime;
            _atlas = atlas;
            _frames = frames;
            _elapsedTime = TimeSpan.Zero;
            _index = 0;

            _timeTarget = frames[_index].Item1;
        }

        public void Update() {
            _elapsedTime += _gameTime.DeltaTime;
            if(_elapsedTime >= _timeTarget) {
                _elapsedTime -= _timeTarget;
                _index++;
                _index %= _frames.Length;

                _timeTarget = _frames[_index].Item1;
            }
        }

    }
}
using System;
using System.Diagnostics;

namespace FuckedUpPlatformer.Util
{
    internal class GameTime 
    {
        public TimeSpan DeltaTime => _deltaTime;
        public TimeSpan ElapsedTime => _elapsedTime;

        public float Delta => (float)_deltaTime.TotalSeconds;

        private TimeSpan _elapsedTime;
        private TimeSpan _deltaTime;
        private Stopwatch _sw;

        public GameTime() 
        {
            _elapsedTime = TimeSpan.Zero;
            _deltaTime = TimeSpan.Zero;
            _sw = Stopwatch.StartNew();
        }

        public void Update()
        {
            TimeSpan currentTime = _sw.Elapsed;
            _deltaTime = currentTime - _elapsedTime;
            _elapsedTime = currentTime;
        }
    }
}
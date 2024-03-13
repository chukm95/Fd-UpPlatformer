using OpenTK.Mathematics;

namespace FuckedUpPlatformer.GameObjects.Map
{
    internal class Chunk
    {
        public const int CHUNK_SIZE = 32;

        public readonly int _x;
        public readonly int _y;
        public int[,] _tiles;
        public bool _hasChanged;

        public Chunk(int x, int y) {
            _x = x; 
            _y = y;
            _tiles = new int[CHUNK_SIZE, CHUNK_SIZE];
        }

        public void Unload() {

        }
    }
}
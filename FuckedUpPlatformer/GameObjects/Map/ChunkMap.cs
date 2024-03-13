using FuckedUpPlatformer.Graphics.Batching;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace FuckedUpPlatformer.GameObjects.Map {
    internal class ChunkMap {
        private Dictionary<(int x, int y), Chunk> _chunks;
        private Dictionary<(int x, int y), SpriteBatcher> _spriteBatchers;
        private Queue<SpriteBatcher> _unusedSpriteBatchers;

        public ChunkMap() {
            _chunks = new Dictionary<(int x, int y), Chunk>();
            _spriteBatchers = new Dictionary<(int x, int y), SpriteBatcher>();
            _unusedSpriteBatchers = new Queue<SpriteBatcher>();
        }

        public Chunk GetOrCreateChunk(int x, int y) {
            if(!_chunks.TryGetValue((x, y), out var chunk)) {
                chunk = new Chunk(x, y);
                _chunks.Add((x, y), chunk);
                _spriteBatchers.Add((x, y), GetOrCreateBatcher());
            }
            return chunk;
        }

        public Chunk GetChunk(int x, int y) {
            _chunks.TryGetValue((x, y), out var chunk);
            return chunk;
        }

        private SpriteBatcher GetOrCreateBatcher() {
            if (_unusedSpriteBatchers.TryDequeue(out var spriteBatcher)) {
                return spriteBatcher;
            }
            else {
                return new SpriteBatcher(Chunk.CHUNK_SIZE, BufferUsageHint.StaticDraw);
            }
        }
    }
}
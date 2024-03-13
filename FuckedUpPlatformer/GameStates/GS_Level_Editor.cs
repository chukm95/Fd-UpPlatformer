using FuckedUpPlatformer.GameObjects.Map;
using FuckedUpPlatformer.GameStateManagement;
using FuckedUpPlatformer.Graphics;
using FuckedUpPlatformer.Graphics.Cameras;
using OpenTK.Mathematics;
using System;

namespace FuckedUpPlatformer.GameStates {
    internal class GS_Level_Editor : IGameState {
        public Enum GameStateId => GameStateIdentifierz.LEVEL_EDITOR;

        private OrthographicCamera _camera;
        private CameraController _cameraController;

        public GS_Level_Editor() {
            _camera = new OrthographicCamera(Vector3.Zero, Vector3.Zero, 1920, 1080, 1,  1000);
            _cameraController = new CameraController(Core.NativeWindow, _camera);
        }

        public void Activate() {
            ChunkMap gm = new ChunkMap();
        }

        public void Update() {

        }

        public void Deactivate() {

        }

        public void Dispose() {

        }

    }
}

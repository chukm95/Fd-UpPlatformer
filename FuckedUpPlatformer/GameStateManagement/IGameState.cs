using System;

namespace FuckedUpPlatformer.GameStateManagement {
    internal interface IGameState {
        Enum GameStateId { get; }
        void Activate();
        void Update();
        void Deactivate();
        void Dispose();
    }
}
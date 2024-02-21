using System;
using System.Collections.Generic;

namespace FuckedUpPlatformer.GameStateManagement {


    internal class GameStateManager {
        private Dictionary<Enum, IGameState> _gameStates;
        private IGameState _currentState;
        private IGameState _nextState;

        internal GameStateManager() {
            _gameStates = new Dictionary<Enum, IGameState>();
            _currentState = null;
            _nextState = null;
        }

        internal void Add(IGameState state) {
            if (!_gameStates.ContainsKey(state.GameStateId)) {
                _gameStates.Add(state.GameStateId, state);
            }
        }

        internal void Set(Enum gameStateId) {
            if (gameStateId != _currentState?.GameStateId && _gameStates.ContainsKey(gameStateId)) {
                _nextState = _gameStates[gameStateId];
            }
        }

        internal void Update() {
            if (_nextState != null) {
                if (_gameStates.ContainsKey(_nextState.GameStateId)) {
                    _currentState?.Deactivate();
                    _currentState = _nextState;
                    _currentState?.Activate();
                    _nextState = null;
                }
            }

            _currentState?.Update();
        }

        internal void Dispose() {
            foreach (var state in _gameStates.Values) {
                state.Dispose();
            }
        }
    }
}
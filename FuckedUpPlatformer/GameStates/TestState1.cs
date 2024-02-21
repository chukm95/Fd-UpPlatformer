using FuckedUpPlatformer.GameStateManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuckedUpPlatformer.GameStates {
    internal class TestState1 : IGameState {
        public Enum GameStateId => GameStateIdentifierz.TEST1;

        public void Activate() {
            Console.WriteLine("TEST 1 # Activate");
        }

        public void Update() {
            Console.WriteLine("TEST 1 # Update");
        }

        public void Deactivate() {
            Console.WriteLine("TEST 1 # Deactivate");
        }

        public void Dispose() {
            Console.WriteLine("TEST 1 # Dispose");
        }

    }
}
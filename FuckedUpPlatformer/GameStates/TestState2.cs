using FuckedUpPlatformer.GameStateManagement;
using System;

namespace FuckedUpPlatformer.GameStates {
    internal class TestState2 : IGameState{
        public Enum GameStateId => GameStateIdentifierz.TEST2;
        int x = 0;

        public void Activate()
        {
            Console.WriteLine("Test state 2 activate!");
        }

        public void Update()
        {
            Console.WriteLine("Test state 2 update!");
            x++;

            if(x == 50000)
                Core.GameStateManager.Set(GameStateIdentifierz.TEST1);
        }

        public void Deactivate()
        {
            Console.WriteLine("Test state 2 decativate!");

        }

        public void Dispose()
        {
            Console.WriteLine("Test state 2 dispose!");

        }
    }
}

using System;
using Observer;
using Tech.State_Machine;
using VContainer;
using VContainer.Unity;

namespace Core.GameLoop
{
    public class Gameloop : IInitializable, IDisposable, ITickable
    {
        [Inject] public EventManager EventManager { get; private set; }
        private StateMachine<GameState, GameStateBase> _stateMachine;
        public void NextDay()
        {
            EventManager.NextDay();
        }

        public void Initialize()
        {
            _stateMachine = new StateMachine<GameState, GameStateBase>();
            _stateMachine.AddNewState(GameState.Combat, new PlayingState(_stateMachine));
            _stateMachine.AddNewState(GameState.Lose, new LoseState(_stateMachine));
            _stateMachine.AddNewState(GameState.Win, new WinState(_stateMachine));
            _stateMachine.AddNewState(GameState.WaitCombat, new WaitCombatState(_stateMachine));
            _stateMachine.Initialize(GameState.WaitCombat);
            GameAction.OnNextDay += NextDay;
        }

        public void Dispose()
        {
            GameAction.OnNextDay -= NextDay;
        }

        public void Tick()
        {
            _stateMachine.CurrentState.OnUpdate();
        }
    }
}


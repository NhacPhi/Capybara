using Tech.State_Machine;

namespace Core.GameLoop
{
    public class WinState : GameStateBase
    {
        public WinState(StateMachine<GameState, GameStateBase> stateMachine) : base(stateMachine)
        {
        }
    }
}
using Tech.State_Machine;

namespace Core.GameLoop
{
    public class LoseState : GameStateBase
    {
        public LoseState(StateMachine<GameState, GameStateBase> stateMachine) : base(stateMachine)
        {
        }
    }
}
using Tech.State_Machine;

namespace Core.GameLoop
{
    public class PlayingState : GameStateBase
    {
        public PlayingState(StateMachine<GameState, GameStateBase> stateMachine) : base(stateMachine)
        {
            
        }
    }
}
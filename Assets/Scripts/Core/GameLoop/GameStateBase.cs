using Tech.State_Machine;

namespace Core.GameLoop
{
    public class GameStateBase : BaseState
    {
        protected StateMachine<GameState, GameStateBase> stateMachine;
        public GameStateBase(StateMachine<GameState, GameStateBase> stateMachine)
        {
            this.stateMachine = stateMachine;
        }
        public override void Enter()
        {
            
        }

        public override void Exit()
        {
            
        }
        
        public virtual void OnUpdate(){}
    }
}
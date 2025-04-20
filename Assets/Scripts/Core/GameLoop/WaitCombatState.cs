using Observer;
using Tech.State_Machine;

namespace Core.GameLoop
{
    public class WaitCombatState : GameStateBase
    {
        public WaitCombatState(StateMachine<GameState, GameStateBase> stateMachine) : base(stateMachine)
        {
            EventAction.OnStartCombat += SwitchToCombatState;
        }

        ~WaitCombatState()
        {
            EventAction.OnStartCombat -= SwitchToCombatState;
        }
        
        private void SwitchToCombatState()
        {
            this.stateMachine.ChangeState(GameState.Combat);
        }
    }
}
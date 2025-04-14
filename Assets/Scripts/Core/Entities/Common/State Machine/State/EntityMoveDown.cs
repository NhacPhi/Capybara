using Core.Entities.Common;

namespace Core.Entities.Player
{
    public class EntityMoveDown : EntityMove
    {
        public EntityMoveDown(EntityStateData data) : base(data)
        {
        }
        
        public override void LogicUpdate()
        {
            if(!MoveToTarget()) return;
            
            data.StateManager.ChangeState(EntityState.IDLE);
        }

        public override void Exit()
        {
            base.Exit();
            data.EndTurn();
        }
    }
}

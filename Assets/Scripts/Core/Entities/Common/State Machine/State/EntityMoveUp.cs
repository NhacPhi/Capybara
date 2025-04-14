using Core.Entities.Common;

namespace Core.Entities.Player
{
    public class EntityMoveUp : EntityMove
    {
        public EntityMoveUp(EntityStateData data) : base(data)
        {
        }

        public override void Enter()
        {
            base.Enter();
            data.Anim.OrderInLayer++;
        }

        public override void LogicUpdate()
        {
            if(!MoveToTarget()) return;
            
            data.StateManager.ChangeState(EntityState.ATTACK);
        }
    }
}

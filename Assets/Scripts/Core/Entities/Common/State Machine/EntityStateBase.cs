using Core.Entities.Common;
using Tech.State_Machine;

namespace Core.Entities
{
    public abstract class EntityStateBase : BaseState
    {
        protected EntityStateData data;

        public EntityStateBase(EntityStateData data)
        {
            this.data = data;            
        }
        
        public override void Enter(){}
        public override void Exit(){}
        public virtual void LogicUpdate(){}
    }
}

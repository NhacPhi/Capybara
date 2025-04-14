using Core.Entities.Common;
using Tech.Pooling;
using UnityEngine;

namespace Core.Entities.Player
{
    public class EntityMove : EntityStateBase
    {
        public EntityMove(EntityStateData data) : base(data)
        {
        }

        public override void Enter()
        {
            if(data.MoveAnim.Equals(string.Empty)) return;
            
            var animData = GenericPool<AnimationData>.Get().Renew();
            animData.Transition = 0.05f;
            animData.IsLoop = true;
            animData.AnimationName = data.MoveAnim; 
            data.Anim.Play(animData);
            GenericPool<AnimationData>.Return(animData);
        }
        
        protected virtual bool MoveToTarget()
        {
            var entityTransform = data.Entity.transform;
            var targetPos = data.MovePosition;
            
            entityTransform.position = Vector2.MoveTowards(entityTransform.position, 
                targetPos, data.MoveSpeed * Time.deltaTime);
            
            return Vector3.Distance(entityTransform.position,  targetPos) < 0.01f;
        }
        
        public override void Exit()
        {
            data.Entity.transform.position = data.MovePosition;
        }
    }
}

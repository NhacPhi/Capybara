using Cysharp.Threading.Tasks;
using Tech.Pooling;
using UnityEngine;

namespace Core.Entities.Common
{
    public class EntityTakeHit : EntityStateBase
    {
        public EntityTakeHit(EntityStateData data) : base(data)
        {
            _ = WaitInit();
        }

        public override void Enter()
        {
            var animData = GenericPool<AnimationData>.Get().Renew();
            animData.Transition = 0.05f;
            animData.AnimationName = data.HitAnim; 
            data.Anim.Play(animData);
            GenericPool<AnimationData>.Return(animData);
            _ = WaitToReturnIdle();
        }

        protected virtual async UniTaskVoid WaitToReturnIdle()
        {
            await UniTask.WaitForSeconds(0.2f);
            data.StateManager.ChangeState(EntityState.IDLE);
        }

        protected async UniTaskVoid WaitInit()
        {
            await UniTask.Yield();
            data.Entity.GetCoreComponent<EntityStats>().OnHit += (_, _) =>
            {
                data.StateManager.ChangeState(EntityState.HIT);
            };
        }
    }
}

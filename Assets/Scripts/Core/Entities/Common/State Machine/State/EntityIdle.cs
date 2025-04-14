using Core.Entities.Common;
using Tech.Pooling;

namespace Core.Entities.Player
{
    public class EntityIdle : EntityStateBase
    {
        public EntityIdle(EntityStateData data) : base(data)
        {
        }
        
        public override void Enter()
        {
            var animData = GenericPool<AnimationData>.Get().Renew();
            animData.IsLoop = true;
            animData.AnimationName = data.IdleAnim;
            data.Anim.Play(animData);
            GenericPool<AnimationData>.Return(animData);
            data.Anim.OrderInLayer = 0;
        }
    }
}

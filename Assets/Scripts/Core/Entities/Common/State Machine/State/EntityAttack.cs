using System;
using Core.Damage;
using Core.Entities.Common;
using Tech.Pooling;
using UnityEngine;

namespace Core.Entities.Player
{
    public class EntityAttack : EntityStateBase
    {
        protected Action damageCallback;
        protected Action exitCallback;
        
        public EntityAttack(EntityStateData data) : base(data)
        {
            damageCallback = () =>
            {
                DamageFormular.DealDamage(DamageBonus.GetDefault(), data.Entity, data.CurrentTarget);
            };

            exitCallback = () =>
            {
                data.StateManager.ChangeState(EntityState.MOVE_DOWN);
            };
        }

        public override void Enter()
        {
            var animData = GenericPool<AnimationData>.Get().Renew();
            animData.AnimationName = data.AttackAnim;
            animData.Transition = 0.05f;
            data.Anim.Play(animData);
            
            data.Anim.RegisterEventAtTime(0.5f, damageCallback);
            data.Anim.RegisterEventAtTime(0.96f, exitCallback);
        }

        public override void Exit()
        {
            data.MovePosition = data.RootPosition;
        }
    }
}

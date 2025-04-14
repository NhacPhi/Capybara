using System;
using Core.Entities.Common;
using Stats;
using Stats.M_Attribute;
using Stats.stat;
using UnityEngine;
using VContainer;

namespace Core
{
    public class EntityStats : StatsController, IDamagable
    {
        public bool IsDead { get; protected set; }
        public Action OnDeath { get; set; }
        public Action<float, Transform> OnHit { get; set; }
        public EntitySkill Skill { get; protected set; }

        protected virtual void Start()
        {
            Skill = this.core.GetCoreComponent<EntitySkill>();
        }

        public virtual void TakeDamage(float damage, Transform attacker)
        {
            OnHit?.Invoke(damage, attacker);
            var hp = GetAttribute(AttributeType.Hp);
            hp.Value -= damage;
            
            if (!(hp.Value <= 0)) return;
            
            HandleDeath();
        }

        protected virtual void HandleDeath()
        {
            IsDead = true;
            OnDeath?.Invoke();
        }
        
        public override void Renew()
        {
            base.Renew();
            IsDead = false;
        }
    }
}
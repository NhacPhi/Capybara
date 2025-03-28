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
        public Action OnDead;
        public Action<DamageInfo> OnHit;
        public bool IsDead { get; protected set; }
        public EntitySkill Skill { get; protected set; }
        [Inject] public TextCombat TextCombat { get; private set; }

        protected virtual void Start()
        {
            Skill = this.core.GetCoreComponent<EntitySkill>();
        }

        public virtual void TakeDamage(DamageInfo damageInfo)
        {
            OnHit?.Invoke(damageInfo);
            var hp = GetAttribute(AttributeType.Hp);
            var def = GetStat(StatType.Def);
            var damageOutput = Mathf.Clamp(damageInfo.Value - def.Value, 1f, int.MaxValue);

            if (Skill)
            {
                Skill.ApplyDefenseSkill(ref damageOutput, damageInfo.Source, this);
            }
            
            TextCombat.CreateDamagePopup(damageOutput, this.transform.position);
            hp.Value -= damageOutput;
            if (!(hp.Value <= 0)) return;
            OnDeath();
        }

        protected virtual void OnDeath()
        {
            IsDead = true;
            OnDead?.Invoke();
        }
        
        public override void Renew()
        {
            base.Renew();
            IsDead = false;
        }
    }
}
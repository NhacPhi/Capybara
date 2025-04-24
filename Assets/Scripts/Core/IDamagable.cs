using System;
using Stats.M_Attribute;
using Stats.stat;
using UnityEngine;
using Attribute = Stats.M_Attribute.Attribute;

namespace Core
{
    public interface IDamagable
    {
        public bool IsDead { get; }
        public Action OnDeath { get; set; }
        public Action<float, Transform> OnHit { get; set; }
        public void TakeDamage(float damage, Transform attacker);
        public Attribute GetAttribute(AttributeType type);
        public Stat GetStat(StatType type);
    }
}
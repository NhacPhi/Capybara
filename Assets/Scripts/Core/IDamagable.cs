using System;
using UnityEngine;

namespace Core
{
    public interface IDamagable
    {
        public bool IsDead { get; }
        public Action OnDeath { get; set; }
        public Action<float, Transform> OnHit { get; set; }
        public void TakeDamage(float damage, Transform attacker);
    }
}
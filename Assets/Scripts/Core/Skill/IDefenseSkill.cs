using UnityEngine;

namespace Core.Skill
{
    public interface IDefenseSkill
    {
        public void OnDamaged(Transform attacker, ref float damageInput);
    }
}
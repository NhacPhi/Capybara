using Core.Entities.Common;
using Observer;
using Stats.stat;
using UnityEngine;

namespace Core.Damage
{
    public static class DamageFormular
    {
        public static void DealDamage(DamageBonus damageBonus, Transform source, Transform target)
        {
            if(!source.TryGetComponent(out Tech.Composite.Core sourceCore)) return;
            if(!target.TryGetComponent(out Tech.Composite.Core targetCore)) return;    
            
            DealDamage(damageBonus, sourceCore, targetCore);
        }

        //Damage Not Aplly Any Skill = SourceATk * Multiplier + FlatValue - TargetDef
        public static void DealDamage(DamageBonus damageBonus, Tech.Composite.Core source, Tech.Composite.Core target)
        {
            GetStatsAndSkillSystem(source, out var sourceStats, out var sourceSkill);
            GetStatsAndSkillSystem(target, out var targetStats, out var targetSkill);
            
            if(!sourceStats || !targetStats) return;
            
            var sourceAtk = sourceStats.GetStat(StatType.Atk);
          
            float damageResult = sourceAtk.Value * damageBonus.DamageMultiplier + damageBonus.FlatValue;

            if (sourceSkill != null)
            {
                sourceSkill.ApplyAttackSkill(ref damageResult);
            }
            
            var targetDef = targetStats.GetStat(StatType.Def);
            
            if (targetSkill)
            {
                targetSkill.ApplyDefenseSkill(ref damageResult, source.transform);
            }
            
            damageResult -= targetDef.Value;
            targetStats.TakeDamage(damageResult, source.transform);
            TextPopupAction.DamagePopup(damageResult, target.transform.position);
        }
        
        private static void GetStatsAndSkillSystem(Tech.Composite.Core core,
            out EntityStats entityStats, out EntitySkill entitySkill)
        {
            entityStats = core.GetCoreComponent<EntityStats>();
            entitySkill = core.GetCoreComponent<EntitySkill>();
        }
    }
}

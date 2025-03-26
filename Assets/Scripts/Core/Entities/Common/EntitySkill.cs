using System.Collections.Generic;
using Core.Skill;
using Tech.Composite;

namespace Core.Entities.Common
{
    public class EntitySkill : CoreComponent
    {
        private Dictionary<string, SkillBase> skillDict = new ();
        
        public void AddSkill(SkillData skill)
        {
            skillDict.Add(skill.ID, skill.CreateRuntimeSkill());            
        }
        
        public bool HasSkill(string skillID)
        {
            return skillDict.Count != 0 && skillDict.ContainsKey(skillID);
        }

        public void ApplyAttackSkill(ref float damage, EntityStats source)
        {
            foreach (var skill in skillDict.Values)
            {
                if (skill.GetSkillType() == SkillType.Attack)
                {
                    skill.OnDamageOutput(ref damage, source);
                }
            }
        }
    }
}
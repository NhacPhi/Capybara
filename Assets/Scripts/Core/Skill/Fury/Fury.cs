using Stats.M_Attribute;
using UnityEngine;

namespace Core.Skill
{
    public class Fury : SkillBase, IAttackSkill
    {
        protected FurySkillData _skillData;

        public Fury(EntityStats owner, FurySkillData skillData) : base(owner)
        {
            _skillData = skillData;
        }

        public override SkillData GetSkillData() => _skillData;

        public void OnDealDamage(ref float damageInput)
        {
            var hp = this.owner.GetAttribute(AttributeType.Hp);
            float ratio = hp.Value / hp.MaxValue;
            float hpPercentActiveSkill = _skillData.Values[0] / 100;
            float dmgPercentAdd = _skillData.Values[1] / 100;
            
            if(ratio > hpPercentActiveSkill) return;

            damageInput *= (1 + dmgPercentAdd);
        }
    }
}
using Stats.M_Attribute;

namespace Core.Skill
{
    public class Fury : SkillBase
    {
        private FurySkillData _skillData;

        public Fury(FurySkillData skillData)
        {
            _skillData = skillData;
        }


        public override SkillType GetSkillType() => SkillType.Attack;

        public override SkillData GetSkillData() => _skillData;

        public override void WhenApplySkil(EntityStats source)
        {
            
        }

        public override void OnDamageOutput(ref float damageInput, EntityStats source)
        {
            var hp = source.GetAttribute(AttributeType.Hp);
            float ratio = hp.Value / hp.MaxValue;
            float ratioHpActiveSkill = 0.5f;
            
            if(ratio > ratioHpActiveSkill) return;

            float damagePercentAdd = 0.15f;
            damageInput *= (1 + damagePercentAdd);
        }
    }
}
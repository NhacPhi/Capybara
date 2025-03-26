namespace Core.Skill
{
    public class Charge : SkillBase
    {
        private ChargeSkillData _skillData;
        public bool IsFirstAttack { get; private set;}
        public Charge(ChargeSkillData skillData)
        {
            _skillData = skillData;
            IsFirstAttack = true;
        }

        public override SkillType GetSkillType() => SkillType.Attack;
        
        public override SkillData GetSkillData() => _skillData;

        public override void WhenApplySkil(EntityStats source)
        {
            //Ignore
        }

        public override void OnDamageOutput(ref float damageInput, EntityStats source)
        {
            if(!IsFirstAttack) return;

            damageInput *= (1 + 0.1f);
            IsFirstAttack = false;
        }
    }
}
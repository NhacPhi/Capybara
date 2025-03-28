namespace Core.Skill
{
    public class Charge : SkillBase, IAttackSkill, IResetSkill
    {
        private ChargeSkillData _skillData;

        public Charge(EntityStats owner, ChargeSkillData skillData) : base(owner)
        {
            _skillData = skillData;
            IsFirstAttack = true;
        }

        public bool IsFirstAttack { get; private set;}

        public override SkillData GetSkillData() => _skillData;

        public void OnDealDamage(ref float damageInput)
        {
            if(!IsFirstAttack) return;
            
            float damagePercentAdd = _skillData.Values[0]; 
            damageInput *= (1 +  damagePercentAdd / 100);
            IsFirstAttack = false;
        }

        public void ResetSkill()
        {
            IsFirstAttack = true;
        }
    }
}
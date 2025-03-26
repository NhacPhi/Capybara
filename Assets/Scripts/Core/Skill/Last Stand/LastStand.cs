namespace Core.Skill
{
    public class LastStand : SkillBase
    {
        private LastStandData data;
        public LastStand(LastStandData data)
        {
            this.data = data;
        }
        
        public override SkillType GetSkillType() => SkillType.Defense;

        public override SkillData GetSkillData() => data;

        public override void WhenApplySkil(EntityStats source)
        {
            
        }

        public override void OnDamageOutput(ref float damageInput, EntityStats source)
        {
            
        }
    }
}
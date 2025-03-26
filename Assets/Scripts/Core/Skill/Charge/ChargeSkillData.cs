namespace Core.Skill
{
    public class ChargeSkillData : SkillData
    {
        public override SkillBase CreateRuntimeSkill() => new Charge(this);
    }
}
namespace Core.Skill
{
    public class LastStandData : SkillData
    {
        public override SkillBase CreateRuntimeSkill() => new LastStand(this);
    }
}
namespace Core.Skill
{
    public class LastStandData : SkillData
    {
        public override SkillBase CreateRuntimeSkill(EntityStats owner) => new LastStand(owner, this);
    }
}
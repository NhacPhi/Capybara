namespace Core.Skill
{
    public class RegenerationData : SkillData
    {
        public override SkillBase CreateRuntimeSkill(EntityStats owner) => new Regeneration(owner, this);
    }
}

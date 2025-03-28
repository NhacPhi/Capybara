namespace Core.Skill
{
    public class FurySkillData : SkillData
    {
        public override SkillBase CreateRuntimeSkill(EntityStats owner) => new Fury(owner, this);
    }
}
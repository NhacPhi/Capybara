namespace Core.Skill
{
    public class FurySkillData : SkillData
    {
        public override SkillBase CreateRuntimeSkill() => new Fury(this);
    }
}
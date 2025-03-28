namespace Core.Skill
{
    public class ResilientHeartData : SkillData
    {
        public override SkillBase CreateRuntimeSkill(EntityStats owner) => new ResilientHeart(owner, this);
    }
}

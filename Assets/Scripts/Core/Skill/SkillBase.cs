namespace Core.Skill
{
    public abstract class SkillBase
    {
        protected EntityStats owner;
        public SkillBase(EntityStats owner)
        {
            this.owner = owner;
        }
        public abstract SkillData GetSkillData();
    }
}
namespace Core.Skill
{
    public abstract class SkillRuntime
    {
        protected EntityStats owner;
        public SkillRuntime(EntityStats owner)
        {
            this.owner = owner;
        }
        public abstract SkillData GetSkillData();
    }
}
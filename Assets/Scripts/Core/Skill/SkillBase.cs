namespace Core.Skill
{
    public abstract class SkillBase
    {
        public abstract SkillType GetSkillType();
        public abstract SkillData GetSkillData();
        public abstract void WhenApplySkil(EntityStats source);
        public abstract void OnDamageOutput(ref float damageInput, EntityStats source);
    }

    public enum SkillType
    {
        Attack,
        Defense,
        Recovery,
        Effect,
    }
}
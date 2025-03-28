using Newtonsoft.Json;

namespace Core.Skill
{
    public class ChargeSkillData : SkillData
    {
        public override SkillBase CreateRuntimeSkill(EntityStats owner) => new Charge(owner, this);
    }
}
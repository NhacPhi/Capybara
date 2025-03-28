using Observer;
using Stats.M_Attribute;

namespace Core.Skill
{
    public class Regeneration : SkillBase
    {
        protected RegenerationData data;
        protected int activeRemaining;

        public Regeneration(EntityStats owner, RegenerationData data) : base(owner)
        {
            this.data = data;
            activeRemaining = (int)data.Values[1];
            GameAction.OnStartCombat += HandleStart;
        }

        ~Regeneration()
        {
            GameAction.OnStartCombat -= HandleStart;
        }
        
        private void HandleStart()
        {
            var hp = this.owner.GetAttribute(AttributeType.Hp);
            var hpPercentHeal = data.Values[0] / 100;
            hp.Value *= (1 + hpPercentHeal);
            activeRemaining--;
        }

        
        
        public override SkillData GetSkillData() => this.data;
        
        
    }
}

using System;
using Observer;
using Stats.M_Attribute;

namespace Core.Skill
{
    public class Regeneration : SkillBase, IDisposable
    {
        protected RegenerationData data;
        protected int activeRemaining;

        public Regeneration(EntityStats owner, RegenerationData data) : base(owner)
        {
            this.data = data;
            activeRemaining = (int)data.Values[1];
            GameAction.OnStartCombat += HandleStart;
        }

        public void Dispose()
        {
            GameAction.OnStartCombat -= HandleStart;
        }

        private void HandleStart()
        {
            if(activeRemaining == 0) return;
            var hp = this.owner.GetAttribute(AttributeType.Hp);
            var hpPercentHeal = data.Values[0] / 100;
            float hpLost = hp.MaxValue - hp.Value;
            float hpHeal = hpLost * hpPercentHeal;
            hp.Value += hpHeal;
            this.owner.TextCombat.CreateHealPopup(hpHeal, this.owner.transform.position);
            activeRemaining--;
        }
        
        public override SkillData GetSkillData() => this.data;
        
        
    }
}

using System;
using Observer;
using Stats.M_Attribute;
using VContainer;

namespace Core.Skill
{
    public class Regeneration : SkillRuntime, IDisposable
    {
        protected RegenerationData data;
        protected int activeRemaining;
        [Inject] private IHealPopup _healPopup;
        
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
            _healPopup.CreateHealPopup(hpHeal, this.owner.transform.position);
            activeRemaining--;
        }
        
        public override SkillData GetSkillData() => this.data;
    }
    
    public class RegenerationData : SkillData
    {
        public override SkillRuntime CreateRuntimeSkill(EntityStats owner) => new Regeneration(owner, this);
    }
}

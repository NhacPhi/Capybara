using System;
using Observer;
using Stats.M_Attribute;
using VContainer;

namespace Core.Skill
{
    public class HealingAura : SkillRuntime, IDisposable
    {
        private HealingAuraData _data;
        private int _roundElapsed;
        private int _roundActiveEffect;
        
        public HealingAura(EntityStats owner, HealingAuraData data) : base(owner)
        {
            _data = data;
            _roundActiveEffect = (int)data.Values[0];
            GameAction.OnRoundChange += HandeRoundChange;
            GameAction.OnStartCombat += HandleStartCombat;
        }

        public void Dispose()
        {
            GameAction.OnRoundChange -= HandeRoundChange;
            GameAction.OnStartCombat -= HandleStartCombat;
        }
        
        private void HandleStartCombat()
        {
            _roundElapsed = 0;
        }
        
        private void HandeRoundChange(int curRound)
        {
            _roundElapsed++;
            if (_roundElapsed < _roundActiveEffect) return;
            
            var hp = owner.GetAttribute(AttributeType.Hp);
            float hpHealPercent = _data.Values[1] / 100;
            float hpAdd = hp.MaxValue * hpHealPercent;
            hp.Value += hpAdd;
            _roundElapsed = 0;
            TextPopupAction.HealPopup?.Invoke(hpAdd, this.owner.transform.position);
        }

        public override SkillData GetSkillData() => _data;

    }

    public class HealingAuraData : SkillData
    {
        public override SkillRuntime CreateRuntimeSkill(EntityStats owner)
        {
            return new HealingAura(owner, this);
        }
    }
}

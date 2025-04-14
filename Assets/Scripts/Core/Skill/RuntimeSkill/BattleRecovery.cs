using System;
using Observer;
using Stats.M_Attribute;
using VContainer;

namespace Core.Skill
{
    public class BattleRecovery : SkillRuntime, IDisposable
    {
        private BattleRecoveryData data;
        
        public BattleRecovery(EntityStats owner, BattleRecoveryData data) : base(owner)
        {
            this.data = data;
            GameAction.OnCombatEnd += HandleCombatEnd;
        }

        public void Dispose()
        {
            GameAction.OnCombatEnd -= HandleCombatEnd;
        }

        public override SkillData GetSkillData() => data;

        private void HandleCombatEnd()
        {
            float hpHealPercent = data.Values[0] / 100;

            var hp = owner.GetAttribute(AttributeType.Hp);
            float hpHeal = hp.MaxValue * hpHealPercent;
            hp.Value += hpHeal;
            TextPopupAction.HealPopup?.Invoke(hpHeal, this.owner.transform.position);
        }
    }

    public class BattleRecoveryData : SkillData
    {
        public override SkillRuntime CreateRuntimeSkill(EntityStats owner)
        {
            return new BattleRecovery(owner, this);
        }
    }
}

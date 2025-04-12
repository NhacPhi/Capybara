using System;
using Observer;
using Stats.M_Attribute;
using Stats.stat;
using VContainer;

namespace Core.Skill
{
    public class VitalityBoost : SkillRuntime, IDisposable
    {
        private VitalityBoostData _data;
        [Inject] private IHealPopup _healPopup;
        private float _hpAdd;
        
        public VitalityBoost(EntityStats owner, VitalityBoostData data) : base(owner)
        {
            _data = data;
            GameAction.OnStartCombat += HandleStartCombat;
            GameAction.OnCombatEnd += HandleCombatEnd;
        }


        public void Dispose()
        {
            GameAction.OnStartCombat -= HandleStartCombat;
            GameAction.OnCombatEnd -= HandleCombatEnd;
        }
        
        public override SkillData GetSkillData() => _data;
        
        private void HandleStartCombat()
        {
            var hp = this.owner.GetAttribute(AttributeType.Hp);
            float maxHpPercent = _data.Values[0] / 100;
            _hpAdd = hp.MaxValue * maxHpPercent;
            
            owner.AddModifier(StatType.MaxHp, new Modifier(_hpAdd));

            hp.Value += _hpAdd;
            _healPopup.CreateHealPopup(_hpAdd, this.owner.transform.position);
        }
        
        private void HandleCombatEnd()
        {
            owner.RemoveModifier(StatType.MaxHp, new Modifier(_hpAdd));    
            this.owner.GetAttribute(AttributeType.Hp).Value -= _hpAdd;
            _healPopup.CreateHealPopup(-_hpAdd, this.owner.transform.position);
        }
    }

    public class VitalityBoostData : SkillData
    {
        public override SkillRuntime CreateRuntimeSkill(EntityStats owner)
        {
            return new VitalityBoost(owner, this);
        }
    }
}

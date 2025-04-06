using System;
using Observer;
using Stats.M_Attribute;
using Stats.stat;
using UnityEngine;

namespace Core.Skill
{
    public class LastStand : SkillBase, IDefenseSkill, IResetSkill, IDisposable
    {
        private LastStandData data;
        private bool _isActive;
        private int _roundRemaining;
        private Stat _defStat;

        public LastStand(EntityStats owner, LastStandData data) : base(owner)
        {
            this.data = data;
            GameAction.OnRoundChange += HandleRoundChange;
        }
        
        public void Dispose()
        {
            GameAction.OnRoundChange -= HandleRoundChange;
        }
        
        private void HandleRoundChange(int curRound, int maxRound)
        {
            _roundRemaining--;
            
            if (_roundRemaining == 0)
            {
                _defStat.RemoveModifier(new Modifier(1, ModifyType.Percent));
            }
        }

        public override SkillData GetSkillData() => data;

        public void OnDamaged(Transform attacker, ref float damageInput)
        {
            if (_isActive) return;
            
            var hp = this.owner.GetAttribute(AttributeType.Hp);
            float hpPercentActive = data.Values[0] / 100;

            if (hp.Value / hp.MaxValue > hpPercentActive) return;
            
            _isActive = true;

            _roundRemaining = (int)data.Values[1];

            _defStat = this.owner.GetStat(StatType.Def);
            damageInput -= _defStat.Value;
            
            _defStat.AddModifier(new Modifier(1, ModifyType.Percent));
        }

        public void ResetSkill()
        {
            _isActive = false;
            _defStat = null;
        }

    }
}
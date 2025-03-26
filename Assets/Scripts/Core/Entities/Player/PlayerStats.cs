using Core;
using Observer;
using Stats.M_Attribute;
using Stats.stat;
using UnityEngine;

namespace Core.Entities.Player
{
    public class PlayerStats : EntityStats
    {
        public override void LoadComponent()
        {
            base.LoadComponent();
            CallEvent();
        }

        private void CallEvent()
        {
            var hp = GetAttribute(AttributeType.Hp);
            var exp = GetAttribute(AttributeType.Exp);
            var expToLvUp = GetStat(StatType.ExpToLevelUp);
            var atk = GetStat(StatType.Atk);
            var maxHp = GetStat(StatType.MaxHp);
            var def = GetStat(StatType.Def);
            var lv = GetAttribute(AttributeType.Lv);
            
            PlayerStatusAction.OnInitHp?.Invoke(new AttributeEvtArgs()
            {
                Attribute = AttributeType.Hp,
                Value = hp.Value,
                MaxValue = hp.MaxValue,
            });
            
            PlayerStatusAction.OnInitExp?.Invoke(new AttributeEvtArgs()
            {
                Attribute = AttributeType.Exp,
                Value = exp.Value,
                MaxValue = exp.MaxValue,
            });
            
            PlayerStatusAction.OnInitLv?.Invoke(new AttributeEvtArgs()
            {
                Attribute = AttributeType.Lv,
                Value = lv.Value,
                MaxValue = lv.MaxValue,
            });
            
            PlayerStatusAction.OnInitAtk?.Invoke(new StatEvtArgs()
            {
                Stat = StatType.Atk,
                Value = atk.Value,
            });
            
            PlayerStatusAction.OnInitDef?.Invoke(new StatEvtArgs()
            {
                Stat = StatType.Def,
                Value = def.Value,
            });

            maxHp.OnValueChange += (args) =>
            {
                PlayerStatusAction.OnMaxHpChange?.Invoke(args);
            };
            
            lv.OnValueChange += (args) =>
            {
                PlayerStatusAction.OnLvChange?.Invoke(args);
            };
            
            hp.OnValueChange += (args) =>
            {
                PlayerStatusAction.OnHpChange?.Invoke(args);
            };

            exp.OnValueChange += (args) =>
            {
                if (Mathf.Approximately(exp.Value, exp.MaxValue))
                {
                    exp.Value = 1;
                    lv.Value ++;
                    expToLvUp.AddModifier(new Modifier(5f));
                }
                PlayerStatusAction.OnExpChange?.Invoke(args);
            };
            
            def.OnValueChange += (args) =>
            {
                PlayerStatusAction.OnDefChange?.Invoke(args);
            };

            atk.OnValueChange += (args) =>
            {
                PlayerStatusAction.OnAtkChange?.Invoke(args);
            };
        }
    }
}


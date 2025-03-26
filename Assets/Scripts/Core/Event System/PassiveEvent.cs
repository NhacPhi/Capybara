using Newtonsoft.Json;
using Stats;
using Stats.M_Attribute;
using Stats.stat;

namespace Event_System
{
    public class PassiveEvent : EventBase
    {
        public override EventType Type => EventType.Passive;

        [JsonProperty("Modifier")]
        public ModifyValue[] ModifyValues { get; private set; } = null;
        public override void HandleEvent(EventManager manager)
        {
            EventHistory eventHistory = manager.EventHistory;
            EventTimeLine eventTimeLine = manager.EventTimeLine;
            eventHistory.CreateMessage()
                .SetDay(eventTimeLine.CurrentDay)
                .SetDescription(this.Description)
                .SetValueChange(this.ModifyValues);
            
            if(ModifyValues == null) return;
            
            if(!manager.Player.TryGetComponent(out StatsController playerStats)) return;
            
            FintLastAttributeAndStatIndex(out var lastAttributeIndex, out var lastStatIndex);
            
            for (var i = 0; i < this.ModifyValues.Length; i++)
            {
                var modifier = this.ModifyValues[i];
               
                if (modifier.IsAttribute)
                {
                    var attribute = playerStats.GetAttribute(modifier.AttributeType);
                    float newValue = 0;
                    switch (modifier.ModType)
                    {
                        case ModifyType.Percent:
                            newValue = attribute.Value * (1 + modifier.Value);
                            break;
                        default:
                            newValue = attribute.Value + modifier.Value;
                            break;
                    }

                    if (i == lastAttributeIndex)
                    {
                        attribute.Value = newValue;   
                    }
                    else
                    {
                        attribute.SetValueWithoutNotify(newValue);
                    }
                    continue;
                }
                
                Modifier statMod = new Modifier()
                {
                    Type = modifier.ModType,
                    Value = modifier.Value
                };
                
                if (i == lastStatIndex)
                {
                    playerStats.AddModifier(modifier.StatType, statMod);
                }
                else
                {
                    playerStats.AddModifierWithoutNotify(modifier.StatType, statMod);
                }
            }
        }

        private void FintLastAttributeAndStatIndex(out int lastAttributeIndex, out int lastStatIndex)
        {
            lastAttributeIndex = 0;
            lastStatIndex = 0;

            for (int i = 0; i < this.ModifyValues.Length; i++)
            {
                var mod = ModifyValues[i];
                
                if (mod.IsAttribute)
                {
                    lastAttributeIndex = i;
                }
                else
                {
                    lastStatIndex = i;
                }
            }
        }
    }

    public class ModifyValue
    {
        [JsonProperty("Attribute")]
        public bool IsAttribute;
        [JsonProperty("Value")]
        public float Value;
        [JsonProperty("ModType")]
        public ModifyType ModType = ModifyType.Constant;
        [JsonProperty("AttributeType")]
        public AttributeType AttributeType;
        [JsonProperty("StatType")]
        public StatType StatType;
    }
}
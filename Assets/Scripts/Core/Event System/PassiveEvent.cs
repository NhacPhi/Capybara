using Core.Entities.Player;
using Newtonsoft.Json;
using Stats.M_Attribute;
using Stats.stat;
using VContainer;

namespace Event_System
{
    public class PassiveEvent : EventBase
    {
        public override EventType Type => EventType.Passive;
        [Inject] protected EventManager eventManager;
        
        [JsonProperty("Modifier")]
        public BaseModifyValue[] ModifyValues { get; private set; } = null;
        
        public override void HandleEvent()
        {
            EventHistory eventHistory = eventManager.EventHistory;
            EventTimeLine eventTimeLine = eventManager.EventTimeLine;
            
            if (eventHistory.CreateMessage().TryGetComponent(out DefaultHistoryItem defaultHistoryItem))
            {
                defaultHistoryItem.SetDay(eventTimeLine.CurrentDay)
                    .SetDescription(this.Description)
                    .SetValueChange(this.ModifyValues);
            }
            
            if(ModifyValues == null) return;
            
            if(!eventManager.Player.TryGetComponent(out PlayerStats playerStats)) return;
            
            foreach (BaseModifyValue modifier in ModifyValues)
            {
                EventUtil.HandleModify(modifier, playerStats);
            }
        }
    }
    
    public abstract class BaseModifyValue
    {
        [JsonProperty("Value")]
        public float Value;
        [JsonProperty("ModType")]
        public ModifyType ModType = ModifyType.Constant;
        public abstract string GetNameOfValue();
    }

    public class AttributeModify : BaseModifyValue
    {
        [JsonProperty("AttributeType")]
        public AttributeType AttributeType;
        public override string GetNameOfValue() => AttributeExtensions.GetName(AttributeType);
    }

    public class StatModify : BaseModifyValue
    {
        [JsonProperty("StatType")]
        public StatType StatType;

        public override string GetNameOfValue() => StatExtensions.GetName(StatType);
    }

    public class MoneyModify : BaseModifyValue
    {
        public const string Money = "Money";
        public override string GetNameOfValue() => Money;
    }
}
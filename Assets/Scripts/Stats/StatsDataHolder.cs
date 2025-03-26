using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Stats.M_Attribute;
using Stats.stat;
using Tech.Logger;

namespace Stats {
    public class StatsDataHolder
    {
        [JsonProperty("Stat")]
        public ReadOnlyDictionary<StatType, float> StatItems;
        [JsonProperty("Attribute")]
        public ReadOnlyDictionary<AttributeType, AttributeItem> AttributeItems;

        public float GetStat(StatType type)
        {
            if(StatItems.TryGetValue(type, out float value))
            {
                return value;
            }
            
            LogCommon.LogWarning($"{type} not Found");
            return default;
        }

        public AttributeItem GetAttribute(AttributeType type)
        {
            if(AttributeItems.TryGetValue(type, out AttributeItem value))
            {
                return value;
            }
            
            LogCommon.LogWarning($"{type} not Found");
            return default;
        }
    }
}
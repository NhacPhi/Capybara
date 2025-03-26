using System;
using Newtonsoft.Json;
using Stats.stat;

namespace Stats.M_Attribute
{
    [Serializable]
    public class AttributeItem
    {
        [JsonProperty("StartPercent")]
        public float StartPercent { get; private set;} = 1f;
        [JsonProperty("Min")]
        public float MinValue { get; private set;} = 0f;
        [JsonProperty("Max")]
        public StatType MaxValue { get; private set;}
    }    
}

using Newtonsoft.Json;
using UnityEngine;

namespace Stats.Status_Effect
{
    /// <summary>
    /// ReadOnly Data Like ScriptableObject
    /// </summary>
    public class EffectData
    {
        [JsonProperty("Name")]
        public string Name { get; protected set; } = string.Empty;
        
        [JsonProperty("Description")]
        public string Description { get; protected set; } = string.Empty;
        
        [JsonProperty("Type")]
        public StatusEffectType EffectType { get; protected set; }
        
        [JsonIgnore]
        public Sprite Icon { get; protected set; } = null;
        
        [JsonProperty("Unique")]
        public bool IsUnique { get; protected set; } = false;
        
        [JsonProperty("Stackable")]
        public bool IsStackable { get; protected set; } = false;
        
        [JsonProperty("MaxStack")]
        public int MaxStack { get; protected set; } = 1;
        
        [JsonProperty("Duration")]
        public float Duration { get; protected set; } = 1f;
    }
}
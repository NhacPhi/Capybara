using Newtonsoft.Json;

namespace Event_System
{
    public abstract class EventBase
    {
        [JsonIgnore] public abstract EventType Type { get; }

        [JsonProperty("ID")]
        public string ID { get; protected set; }
        [JsonProperty("Name")]
        public virtual string Name { get; protected set; } = string.Empty;
        [JsonProperty("Description")]
        public virtual string Description { get; protected set; } = string.Empty;

        public abstract void HandleEvent();
    }
}


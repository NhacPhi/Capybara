using Newtonsoft.Json;

namespace Core.Skill
{
    public abstract class SkillData
    {
        [JsonProperty("ID")]
        public string ID { get; protected set;}
        [JsonProperty("Name")]
        public string Name { get; protected set;}
        [JsonProperty("Description")]
        public string Description { get; protected set;}
        public abstract SkillBase CreateRuntimeSkill();
    }
}
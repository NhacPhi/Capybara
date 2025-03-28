using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Pool;

namespace Core.Skill
{
    public abstract class SkillData
    {
        [JsonProperty("ID")]
        public string ID { get; protected set;}
        [JsonProperty("Name")]
        public string Name { get; protected set;}
        [JsonProperty("Description")]
        protected string description;
        [JsonIgnore]
        public string Description
        {
            get
            {
                if (Values == null || Values.Count == 0)
                    return description;

                var sb1 = GenericPool<StringBuilder>.Get().Clear();
                sb1.Append(description);
                var sb2 = GenericPool<StringBuilder>.Get();
                for (int i = 0; i < Values.Count; i++)
                {
                    sb2.Clear();
                    sb2.Append('{').Append(i).Append('}');
                    sb1.Replace(sb2.ToString(), Values[i].ToString(CultureInfo.InvariantCulture));
                }
                GenericPool<StringBuilder>.Release(sb1);
                GenericPool<StringBuilder>.Release(sb2);
                return sb1.ToString();
            }
        }

        [JsonProperty("Values")] 
        public ReadOnlyCollection<float> Values { get; protected set; }
        public abstract SkillBase CreateRuntimeSkill(EntityStats owner);
    }
}
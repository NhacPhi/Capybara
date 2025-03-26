using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Stats
{
    public class EntitiesStatsDataBase
    {
        [JsonProperty("Entities Stats")]
        public ReadOnlyDictionary<string, StatsDataHolder> EntitiesStats { get; private set; }
    }
}

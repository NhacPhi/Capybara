using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Tech.Json;
using UnityEngine;

namespace Stats
{
    public class EntitiesStatsDataBase
    {
        public Dictionary<string, StatsDataHolder> EntitiesStats { get; private set; }

        public async UniTask Init()
        {
            string key = "EntitiesStats";
            var textAsset = await AddressablesManager.Instance.LoadAssetAsync<TextAsset>(key);
            EntitiesStats = Json.DeserializeObject<Dictionary<string, StatsDataHolder>>(textAsset.text);
            AddressablesManager.Instance.RemoveAsset(key);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Tech.Json;
using UnityEngine;

namespace Core.Skill
{
    public class SkillDatabase
    {
        private static Dictionary<string, SkillData> _data;

        public async UniTask Init(CancellationToken cancellationToken = default)
        {
            var textAsset = await AddressablesManager.Instance.LoadAssetAsync<TextAsset>(
                AddressConstant.SkillDatabase, token: cancellationToken);
            
            var listSKill = Json.DeserializeObject<List<SkillData>>(textAsset.text);
            
            _data = listSKill.ToDictionary(x => x.ID, x => x);
        }
        public int Count => _data.Count;
        public SkillData GetRandomSkill()
        {
            return _data.ElementAt(Random.Range(0, _data.Count)).Value;
        }

        public SkillData GetSkill(int index)
        {
            return _data.ElementAt(index).Value;
        }

        public SkillData GetSkill(string id)
        {
            return _data.GetValueOrDefault(id);
        }
    }
}
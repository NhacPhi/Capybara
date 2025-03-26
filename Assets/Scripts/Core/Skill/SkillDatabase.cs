using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Tech.Json;
using UnityEngine;
using VContainer;

namespace Core.Skill
{
    public class SkillDatabase
    {
        [Inject] private DataService _dataService;
        private static Dictionary<string, SkillData> _data;

        public async UniTask Init(CancellationToken cancellationToken = default)
        {
            var listSKill = await _dataService.LoadDataAsync
                <List<SkillData>>(AddressConstant.SkillDatabase, cancellationToken);

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
    }
}
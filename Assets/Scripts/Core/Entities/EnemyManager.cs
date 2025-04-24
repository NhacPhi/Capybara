using System;
using System.Collections.Generic;
using System.Threading;
using Core.Entities.Common;
using Core.TurnBase;
using Cysharp.Threading.Tasks;
using Observer;
using Tech.Pooling;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core.Entities
{
    public class EnemyManager : IDisposable, IAsyncStartable
    {
        [Inject] private TurnManager _turnManager;
        
        private Dictionary<string, Entity> _enemiesData = new Dictionary<string, Entity>(); 
        private List<Entity> _enemiesToSpawn = new List<Entity>();
        
        public void Dispose()
        {
            EventAction.OnStartCombat -= HandleStartCombat;
        }

        public async UniTask StartAsync(CancellationToken cancellation = default)
        {
            EventAction.OnStartCombat += HandleStartCombat;
            var prefabs = await AddressablesManager.Instance.LoadAssetsAsync<GameObject>("Enemy", token: cancellation);

            foreach (var prefab in prefabs)
            {
                _enemiesData.Add(prefab.name, prefab.GetComponent<Entity>());
                await UniTask.Yield(cancellationToken: cancellation);
            }
        }

        public void AddToSpawnList(string id)
        {
            if(!_enemiesData.TryGetValue(id, out Entity entity)) return;
            
            _enemiesToSpawn.Add(entity);
        }
        
        private void HandleStartCombat()
        {
            foreach (var entity in _enemiesToSpawn)
            {
                var enemy = PoolManager.Instance.SpawnObject(entity, 
                    entity.transform.position, Quaternion.identity);
                
                _turnManager.RegisterEnemy(enemy);
            }
            
            _enemiesToSpawn.Clear();
        }
    }
}
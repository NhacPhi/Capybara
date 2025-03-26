using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core.Entities.Enemy;
using Core.Entities.Player;
using Cysharp.Threading.Tasks;
using Observer;
using Tech.Pooling;
using UnityEngine;

namespace Core.Entities
{
    public class EnemyManager : IDisposable
    {
        private List<EnemyCtrl> _curEnemies = new ();
        private EnemyCtrl _enemyPrefab;
        
        //Test
        private const string _enemyAddress = "enemy_01";
        private Vector3 _enemySpawnPos = new Vector3(1.59f, 2.281f, 0);
        
        public PlayerCtrl Player;
        
        public async UniTask Init(CancellationToken token = default)
        {
            GameAction.OnStartCombat += HandleStartCombat;
            GameAction.OnEnemyDead += HandleEnemyDead;
            GameAction.OnEnemyTurnStart += HandleEnemyTurn;
            var prefab = await AddressablesManager.Instance.LoadAssetAsync<GameObject>(_enemyAddress, token: token);
            _enemyPrefab = prefab.GetComponent<EnemyCtrl>();
        }

        public void Dispose()
        {
            GameAction.OnStartCombat -= HandleStartCombat;
            GameAction.OnEnemyDead -= HandleEnemyDead;
            GameAction.OnEnemyTurnStart -= HandleEnemyTurn;
        }
        
        private void HandleEnemyTurn()
        {
            _ = HandleTurn();
        }

        private async UniTask HandleTurn()
        {
            if (_curEnemies.Count == 0)
            {
                GameAction.OnCombatEnd?.Invoke();
                return;
            }
            
            int enemyIndex = 0;
            bool waitAction = true;
            var cancelToken = GameManager.GlobalTokenOnDestroy;
            
            EnemyCtrl curEnemy = null;
            float waitTime = 0.1f;
            while (enemyIndex <= _curEnemies.Count - 1)
            {
                if (waitAction)
                {
                    curEnemy = _curEnemies[enemyIndex];
                    curEnemy.HandleTurn(this.Player);
                    waitAction = false;
                }
                
                await UniTask.WaitForSeconds(waitTime, cancellationToken: cancelToken);
                
                if(!curEnemy.EndTurn) continue;

                enemyIndex++;
                waitAction = true;
            }
            GameAction.OnEndEnemyTurn?.Invoke();
        }
        
        private void HandleEnemyDead(EnemyCtrl enemy)
        {
            _curEnemies.Remove(enemy);
        }

        private void HandleStartCombat()
        {
            _curEnemies.Add(PoolManager.Instance.SpawnObject(_enemyPrefab, _enemySpawnPos, Quaternion.identity));
        }


        public EnemyCtrl GetFirstEnemy()
        {
            return _curEnemies.FirstOrDefault();
        }
    }
}
using Core.Entities.Common;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Observer;
using UnityEngine;
using VContainer;

namespace Core.Entities.Player
{
    public class PlayerCtrl : Tech.Composite.Core
    {
        [Inject] private EnemyManager _enemyManager;

        public PlayerStats PlayerStats { get; protected set;}
        public AttackSystem AttackSystem {get; protected set;}
        protected override void LoadComponent()
        {
            base.LoadComponent();
            PlayerStats = this.GetCoreComponent<PlayerStats>();
            AttackSystem = this.GetCoreComponent<AttackSystem>();
            GameAction.OnPlayerTurnStart += HandlePlayerTurn;
        }

        private void OnDestroy()
        {
            GameAction.OnPlayerTurnStart -= HandlePlayerTurn;
        }

        private void HandlePlayerTurn()
        {
            _ = WaitCombat();
        }

        private async UniTask WaitCombat()
        {
            float waitTime = 0.15f;
            float timeToMove = 0.3f;
            var cancelToken = GameManager.GlobalTokenOnDestroy;
            await UniTask.WaitForSeconds(waitTime, cancellationToken: cancelToken);
            var enemy = _enemyManager.GetFirstEnemy();
        
            if(enemy == null) return;
       
            Vector3 oldPosition = transform.position;
            Vector3 targetPosition = transform.position;
            targetPosition.x = enemy.transform.position.x;
            transform.DOMove(targetPosition - Vector3.right, timeToMove);   
            await UniTask.WaitForSeconds(timeToMove, cancellationToken: cancelToken);

            if (enemy.TryGetComponent(out IDamagable damagable))
            {
                AttackSystem.DealDamage(damagable);
            }
        
            await UniTask.WaitForSeconds(waitTime, cancellationToken: cancelToken);
        
            transform.DOMove(oldPosition, timeToMove);
        
            await UniTask.WaitForSeconds(timeToMove, cancellationToken: cancelToken);
            GameAction.OnEndPlayerTurn?.Invoke();
        } 
    }
}


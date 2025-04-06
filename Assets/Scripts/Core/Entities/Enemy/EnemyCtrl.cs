using Core.Entities.Player;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Observer;
using Stats.stat;
using UnityEngine;

namespace Core.Entities.Enemy
{
    public class EnemyCtrl : Tech.Composite.Core
    {
        public EnemyStats Stats { get; protected set; }
        public bool EndTurn { get; private set;}

        protected override void LoadComponent()
        {
            base.LoadComponent();
            Stats = this.GetCoreComponent<EnemyStats>();
        }

        private void OnDisable()
        {
            GameAction.OnEnemyDead?.Invoke(this);
        }

        public void HandleTurn(PlayerCtrl player)
        {
            EndTurn = false;
            _ = WaitAttack(player);
        }

        private async UniTask WaitAttack(PlayerCtrl player)
        {
            float waitTime = 0.15f;
            float timeToMove = 0.3f;
            await UniTask.WaitForSeconds(waitTime);
            
            Vector3 oldPosition = transform.position;
            Vector3 targetPosition = transform.position;
            targetPosition.x = player.transform.position.x;
            transform.DOMove(targetPosition + Vector3.right, timeToMove);
            await UniTask.WaitForSeconds(timeToMove);
            
            if(!player.TryGetComponent(out IDamagable damagable)) return;
            
            var damage = Stats.GetStat(StatType.Atk).Value;
            damagable.TakeDamage(new DamageInfo(damage, this.transform));
        
            await UniTask.WaitForSeconds(waitTime);
            
            transform.DOMove(oldPosition, timeToMove);
            
            await UniTask.WaitForSeconds(timeToMove);
            EndTurn = true;
        }
    }
}
using Observer;

namespace Core.Entities.Enemy
{
    public class EnemyStats : EntityStats
    {
        private void OnEnable()
        {
            if(!this.IsDead) return;
            
            Renew();
        }

        protected override void HandleDeath()
        {
            base.HandleDeath();
            GameAction.OnEnemyDead?.Invoke(core as EnemyCtrl);
            gameObject.SetActive(false);
        }
    }
}
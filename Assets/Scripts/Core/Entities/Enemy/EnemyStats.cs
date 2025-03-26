namespace Core.Entities.Enemy
{
    public class EnemyStats : EntityStats
    {
        private void OnEnable()
        {
            if(!this.IsDead) return;
            
            Renew();
        }

        protected override void OnDeath()
        {
            base.OnDeath();
            //Return To Pool
            gameObject.SetActive(false);
        }
    }
}
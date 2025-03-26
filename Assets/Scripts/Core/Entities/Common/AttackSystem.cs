using Stats.stat;
using Tech.Composite;

namespace Core.Entities.Common
{
    public class AttackSystem : CoreComponent
    {
        private EntityStats _stats;
        private EntitySkill _skillSystem;
        private void Start()
        {
            _stats = core.GetCoreComponent<EntityStats>();
            _skillSystem = core.GetCoreComponent<EntitySkill>();
        }

        public void DealDamage(IDamagable target)
        {
            var atk = _stats.GetStat(StatType.Atk);
            var damage = atk.Value;

            if (_skillSystem)
            {
                _skillSystem.ApplyAttackSkill(ref damage, _stats);
            }
            
            target.TakeDamage(new DamageInfo(damage, this.transform));
        }        
    }
}
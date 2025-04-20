using System;
using Core.Entities.Enemy;
using Observer;
using Stats.M_Attribute;
using VContainer;

namespace Core.Skill
{
    public class FatalFinisher : SkillRuntime, IDisposable
    {
        private FatalFinisherData _data;
        
        public FatalFinisher(EntityStats owner, FatalFinisherData data) : base(owner)
        {
            _data = data;
            EventAction.OnEnemyDead += HandleEnemyDead;
        }
        
        public void Dispose()
        {
            EventAction.OnEnemyDead -= HandleEnemyDead;
        }

        private void HandleEnemyDead(Entities.Enemy.EnemyCtrl enemy)
        {
            float hpPercentHeal =_data.Values[0] / 100;
            var hp = owner.GetAttribute(AttributeType.Hp);
            float hpAdd = hp.MaxValue * hpPercentHeal; 
            hp.Value += hpAdd;
            TextPopupAction.HealPopup?.Invoke(hpAdd, this.owner.transform.position);
        }

        public override SkillData GetSkillData() => _data;

    }

    public class FatalFinisherData : SkillData
    {
        public override SkillRuntime CreateRuntimeSkill(EntityStats owner)
        {
            return new FatalFinisher(owner, this);
        }
    }
}

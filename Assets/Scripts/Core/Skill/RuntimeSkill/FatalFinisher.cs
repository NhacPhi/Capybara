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
        [Inject] private IHealPopup _healPopup;
        
        public FatalFinisher(EntityStats owner, FatalFinisherData data) : base(owner)
        {
            _data = data;
            GameAction.OnEnemyDead += HandleEnemyDead;
        }
        
        public void Dispose()
        {
            GameAction.OnEnemyDead -= HandleEnemyDead;
        }

        private void HandleEnemyDead(EnemyCtrl enemy)
        {
            float hpPercentHeal =_data.Values[0] / 100;
            var hp = owner.GetAttribute(AttributeType.Hp);
            float hpAdd = hp.MaxValue * hpPercentHeal; 
            hp.Value += hpAdd;
            _healPopup.CreateHealPopup(hpAdd, owner.transform.position);
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

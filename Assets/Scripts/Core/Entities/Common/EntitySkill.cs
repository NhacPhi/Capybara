using System;
using System.Collections.Generic;
using Core.Skill;
using Observer;
using Tech.Composite;
using UnityEngine;

namespace Core.Entities.Common
{
    public class EntitySkill : CoreComponent
    {
        private Dictionary<string, SkillBase> _skillDict = new ();
        private EntityStats _entityStats; 
            
        protected override void Awake()
        {
            base.Awake();
            GameAction.OnCombatEnd += HandleCombatEnd;
        }

        private void Start()
        {
            _entityStats = core.GetCoreComponent<EntityStats>();
        }

        private void OnDestroy()
        {
            GameAction.OnCombatEnd -= HandleCombatEnd;
            foreach (var skill in _skillDict.Values)
            {
                if (skill is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }

        private void HandleCombatEnd()
        {
            foreach (var skill in _skillDict.Values)
            {
                if (skill is IResetSkill skillReset)
                {
                    skillReset.ResetSkill();
                }
            }
        }

        public void AddSkill(SkillData skill)
        {
            _skillDict.Add(skill.ID, skill.CreateRuntimeSkill(_entityStats));            
        }
        
        public bool HasSkill(string skillID)
        {
            return _skillDict.Count != 0 && _skillDict.ContainsKey(skillID);
        }

        public void ApplyAttackSkill(ref float damage, EntityStats source)
        {
            foreach (var skill in _skillDict.Values)
            {
                if (skill is IAttackSkill attackSkill)
                {
                    attackSkill.OnDealDamage(ref damage);
                }
            }
        }

        public void ApplyDefenseSkill(ref float damage, Transform attacker, EntityStats source)
        {
            foreach (var skill in _skillDict.Values)
            {
                if (skill is IDefenseSkill defenseSkill)
                {
                    defenseSkill.OnDamaged(attacker, ref damage);
                }
            }
        }
    }
}
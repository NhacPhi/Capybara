using System.Collections.Generic;
using Core.Entities.Common;
using Core.Skill;
using Cysharp.Threading.Tasks;
using Observer;
using UnityEngine;
using VContainer;

namespace UI
{
    public class ChooseSkillPanel : PanelBase
    {
        [Inject] private SkillDatabase _database;
        [Inject] private UIManager _uiManager;
        [field: SerializeField] public SkillItemUI[] SkillItems { get; private set; }
        private EventHistory _eventHistory;
        private const string rewardMessage = "Bạn nhận đc kĩ năng : ";
        protected override void OnAwake()
        {
            base.OnAwake();
            SkillItems = GetComponentsInChildren<SkillItemUI>();
            _ = WaitLoading();
        }

        private async UniTask WaitLoading()
        {
            GameplayPanel panel;
            do
            {
                //Wait _uiManager Inject And Panel Created
                await UniTask.Yield();
                panel =  _uiManager.GetPanel<GameplayPanel>(AddressConstant.GameplayPanel);
            } 
            while (!panel);
            
            _eventHistory = panel.EventHistory;
            foreach (var skillItem in SkillItems)
            {
                skillItem.Btn.onClick.AddListener(() =>
                {
                    skillItem.entities.AddSkill(skillItem.SkillData);
                    this.Hide();
                _eventHistory.CreateMessage().SetDescription(rewardMessage + skillItem.SkillData.Name);
                GameAction.OnSelectionEventDone?.Invoke();
                });
            }
        }
        
        public void Populate(EntitySkill entities)
        {
            var skillIds = new HashSet<int>();
            foreach (var skillItem in SkillItems)
            {
                int index = Random.Range(0, _database.Count);
                
                while (skillIds.Contains(index))
                {
                    index = Random.Range(0, _database.Count);
                }

                skillIds.Add(index);

                var skill = _database.GetSkill(index);
                
                if (entities.HasSkill(skill.ID))
                {
                    skillItem.gameObject.SetActive(false);
                    continue;
                }
                
                skillItem.gameObject.SetActive(true);
                skillItem.SkillName.text = skill.Name;
                skillItem.Description.text = skill.Description;
                skillItem.SkillData = skill;
                skillItem.entities = entities;
            }
        }
    }
}
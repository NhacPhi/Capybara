using System.Collections.Generic;
using Core.Entities.Common;
using Core.Skill;
using Cysharp.Threading.Tasks;
using Observer;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

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
            GameplayPanel gameplayPanel;
            do
            {
                //Wait _uiManager Inject And Panel Created
                await UniTask.Yield();
                gameplayPanel =  _uiManager.GetPanel<GameplayPanel>(AddressConstant.GameplayPanel);
            } 
            while (!gameplayPanel);
            
            _eventHistory = gameplayPanel.EventHistory;
            foreach (var skillItem in SkillItems)
            {
                skillItem.Btn.onClick.AddListener(() =>
                {
                    skillItem.entities.AddSkill(skillItem.SkillData);
                    this.Hide();
                    
                    gameplayPanel.EventButton.NextDayBtn.gameObject.SetActive(true);
                    var message = gameplayPanel.EventHistory.CreateMessage(MessageType.Default);
                    if (message.TryGetComponent(out DefaultHistoryItem item))
                    {
                        item.SetDescription(rewardMessage + skillItem.SkillData.Name);
                    }
                    
                    EventAction.OnSelectionEventDone?.Invoke();
                });
            }
        }
        
        public void PopulateRandomSkill(EntitySkill entities)
        {
            var skillIds = new HashSet<int>();
            var skill1 = _database.GetSkill("skill_09");
            SkillItems[0].gameObject.SetActive(true);
            SkillItems[0].SkillName.text = skill1.Name;
            SkillItems[0].Description.text = skill1.Description;
            SkillItems[0].SkillData = skill1;
            SkillItems[0].entities = entities;
            
            for (var i = 1; i < SkillItems.Length; i++)
            {
                SkillItemUI skillItem = SkillItems[i];
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

        public void Populate(EntitySkill entities, SkillData[] skills)
        {
            for (var i = 0; i < SkillItems.Length; i++)
            {
                var skill = skills[i];
                SkillItemUI skillItem = SkillItems[i];
                skillItem.gameObject.SetActive(true);
                skillItem.SkillName.text = skill.Name;
                skillItem.Description.text = skill.Description;
                skillItem.SkillData = skill;
                skillItem.entities = entities;
            }
        }
    }
}
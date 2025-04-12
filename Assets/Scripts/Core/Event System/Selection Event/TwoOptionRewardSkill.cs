using System.Text;
using Core.Entities.Common;
using Core.Entities.Player;
using Newtonsoft.Json;
using Observer;
using Stats.stat;
using Tech.Pooling;
using UI;
using UI.UI_Manager.Button;
using VContainer;

namespace Event_System
{
    public class TwoOptionRewardSkill : EventBase
    {
        public override EventType Type => EventType.Special;
        [JsonProperty("Option 1")]
        public SelectOption Option1 { get; private set; }
        [JsonProperty("Option 2")]
        public SelectOption Option2 { get; private set; }
        [Inject] protected EventManager eventManager;
        
        public override void HandleEvent()
        {
            EventHistory eventHistory = eventManager.EventHistory;
            EventTimeLine eventTimeLine = eventManager.EventTimeLine;
            
            if (eventHistory.CreateMessage().TryGetComponent(out DefaultHistoryItem defaultHistoryItem))
            {
                defaultHistoryItem.SetDay(eventTimeLine.CurrentDay)
                    .SetDescription(this.Description);
            }
            
            var eventBtn = eventManager.UIManager.GetFirstPanelOfType<GameplayPanel>().EventButton;
            var selectionBtn1 = eventBtn.SelectionGroup.SelectionBtn1;
            var selectionBtn2 = eventBtn.SelectionGroup.SelectionBtn2;
            
            SetUpOption(selectionBtn1, Option1, eventManager);
            SetUpOption(selectionBtn2, Option2, eventManager);
        }

        private void SetUpOption(SelectionBtn selectionBtn, SelectOption option, EventManager manager)
        {
            selectionBtn.Title_TMP.text = option.Name;
            var modifier = option.ModifyValue;
            var stringBuilder = GenericPool<StringBuilder>.Get().Clear();
            var playerStats = manager.Player.GetComponent<PlayerStats>();
            var playerSkill = manager.Player.GetCoreComponent<EntitySkill>();

            selectionBtn.Btn.onClick.RemoveAllListeners();
            switch (modifier)
            {
                //Closure allocation
                case AttributeModify attributeModify:
                    selectionBtn.Btn.onClick.AddListener(() =>
                    {
                        EventUtil.HandleAttribute(attributeModify, playerStats);
                        PopulatePanel(manager.UIManager, playerSkill);
                    });
                    break;
                case StatModify statModify:
                    selectionBtn.Btn.onClick.AddListener(() =>
                    {
                        EventUtil.HandleStat(statModify, playerStats);
                        PopulatePanel(manager.UIManager, playerSkill);
                    });
                    break;
                case MoneyModify moneyModify:
                    selectionBtn.Btn.onClick.AddListener(() =>
                    {
                        EventUtil.HandleMoney(moneyModify, playerStats);
                        PopulatePanel(manager.UIManager, playerSkill);
                    });
                    break;
            }
            
            BuildingText(stringBuilder, modifier);
            selectionBtn.ValueChange_TMP.text = stringBuilder.ToString();
            GenericPool<StringBuilder>.Return(stringBuilder);
        }

        private void PopulatePanel(UIManager uiManager, EntitySkill skill)
        {
            GameAction.OnPlayerSelect?.Invoke();
            var panel = uiManager.GetFirstPanelOfType<ChooseSkillPanel>();
            panel.Show();
            panel.PopulateRandomSkill(skill);
        }
        
        private void BuildingText(StringBuilder stringBuilder, BaseModifyValue modifier)
        {
            stringBuilder.Append(modifier.GetNameOfValue());
            stringBuilder.Append(' ');
                
            if (modifier.Value > 0)
            {
                stringBuilder.Append('+');
            }
                
            stringBuilder.Append(modifier.Value);

            if (modifier.ModType == ModifyType.Percent)
            {
                stringBuilder.Append('%');
            }
        }
    }

    public class SelectOption
    {
        [JsonProperty("Name")]
        public string Name { get; private set; }
        [JsonProperty("Modifier")]
        public BaseModifyValue ModifyValue { get; private set; }
    }
}
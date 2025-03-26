using System.Text;
using Core.Entities.Common;
using Newtonsoft.Json;
using Observer;
using Stats;
using Stats.M_Attribute;
using Stats.stat;
using Tech.Pooling;
using UI;
using UI.UI_Manager.Button;

namespace Event_System
{
    public class SelectionEvent : EventBase
    {
        public override EventType Type => EventType.Selection;
        [JsonProperty("Option 1")]
        public SelectOption Option1 { get; private set; }
        [JsonProperty("Option 2")]
        public SelectOption Option2 { get; private set; }
        
        public override void HandleEvent(EventManager manager)
        {
            EventHistory eventHistory = manager.EventHistory;
            EventTimeLine eventTimeLine = manager.EventTimeLine;
            eventHistory.CreateMessage()
                .SetDay(eventTimeLine.CurrentDay)
                .SetDescription(this.Description);
            
            var eventBtn = manager.UIManager.GetFirstPanelOfType<GameplayPanel>().EventButton;
            var selectionBtn1 = eventBtn.SelectionGroup.SelectionBtn1;
            var selectionBtn2 = eventBtn.SelectionGroup.SelectionBtn2;
            
            SetUpOption(selectionBtn1, Option1, manager);
            SetUpOption(selectionBtn2, Option2, manager);
        }

        private void SetUpOption(SelectionBtn selectionBtn, SelectOption option, EventManager manager)
        {
            selectionBtn.Title_TMP.text = option.Name;
            var modifier = option.ModifyValue;
            var stringBuilder = GenericPool<StringBuilder>.Get().Clear();
            var playerStat = manager.Player.GetComponent<StatsController>();
            var playerSkill = manager.Player.GetCoreComponent<EntitySkill>();
                
            if (modifier.IsAttribute)
            {
                var attributeName = AttributeExtensions.GetName(modifier.AttributeType);
                selectionBtn.Btn.onClick.RemoveAllListeners();
                selectionBtn.Btn.onClick.AddListener(() =>
                {
                    var attribute = playerStat.GetAttribute(modifier.AttributeType);
                    switch (modifier.ModType)
                    {
                        case ModifyType.Percent:
                            attribute.Value *= (1 + modifier.Value);
                            break;
                        default:
                            attribute.Value += modifier.Value;
                            break;
                    }
                    GameAction.OnPlayerSelect?.Invoke();
                    var panel = manager.UIManager.GetFirstPanelOfType<ChooseSkillPanel>();
                    panel.Show();
                    panel.Populate(playerSkill);
                });
                BuildingText(stringBuilder, attributeName, modifier);
            }
            else
            {
                var statName = StatExtensions.GetName(modifier.StatType);
                selectionBtn.Btn.onClick.RemoveAllListeners();
                selectionBtn.Btn.onClick.AddListener(() =>
                {
                    playerStat.AddModifier(modifier.StatType, new Modifier(modifier.Value, modifier.ModType));
                    GameAction.OnPlayerSelect?.Invoke();
                    var panel = manager.UIManager.GetPanel<ChooseSkillPanel>(AddressConstant.ChooseSkillPanel);
                    panel.Show();
                    panel.Populate(playerSkill);
                });
                BuildingText(stringBuilder, statName, modifier);
            }
            
            selectionBtn.ValueChange_TMP.text = stringBuilder.ToString();
            GenericPool<StringBuilder>.Return(stringBuilder);
        }

        private void BuildingText(StringBuilder stringBuilder, string valueName, ModifyValue modifier)
        {
            stringBuilder.Append(valueName);
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
        public ModifyValue ModifyValue { get; private set; }
    }
}
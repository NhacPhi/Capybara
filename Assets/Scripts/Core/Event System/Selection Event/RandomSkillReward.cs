using Core.Entities.Common;
using Event_System;
using UI;
using VContainer;

namespace Event_System
{
    public class RandomSkillReward : EventBase
    {
        public override EventType Type => EventType.Special;
        [Inject] protected EventManager eventManager;
        
        public override void HandleEvent()
        {
            var gamePanel = eventManager.UIManager.GetFirstPanelOfType<GameplayPanel>();
            var panel = eventManager.UIManager.ShowPanel<ChooseSkillPanel>(AddressConstant.ChooseSkillPanel);

            panel.PopulateRandomSkill(eventManager.Player.GetComponent<EntitySkill>());
        }
    }
}
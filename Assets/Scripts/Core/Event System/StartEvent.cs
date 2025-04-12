using VContainer;

namespace Event_System
{
    public class StartEvent : EventBase
    {
        public override EventType Type => EventType.Start;
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
        }
    }
}
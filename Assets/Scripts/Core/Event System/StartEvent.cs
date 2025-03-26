namespace Event_System
{
    public class StartEvent : EventBase
    {
        public override EventType Type { get; }
        public override void HandleEvent(EventManager manager)
        {
            EventHistory eventHistory = manager.EventHistory;
            EventTimeLine eventTimeLine = manager.EventTimeLine;
            eventHistory.CreateMessage()
                .SetDay(eventTimeLine.CurrentDay)
                .SetDescription(this.Description);
        }
    }
}
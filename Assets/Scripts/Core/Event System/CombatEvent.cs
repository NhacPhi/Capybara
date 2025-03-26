namespace Event_System
{
    public class CombatEvent : EventBase
    {
        public override EventType Type => EventType.Fight;
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
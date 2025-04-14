using Core.Entities;
using Newtonsoft.Json;
using UnityEngine;
using VContainer;

namespace Event_System
{
    public class CombatEvent : EventBase
    {
        [Inject] protected EventManager eventManager;
        public override EventType Type => EventType.Fight;
        [JsonProperty("EnemyID")]
        public string EnemyID;
        [Inject] protected EnemyManager enemyManager;
        
        public override void HandleEvent()
        {
            EventHistory eventHistory = eventManager.EventHistory;
            EventTimeLine eventTimeLine = eventManager.EventTimeLine;

            if (eventHistory.CreateMessage().TryGetComponent(out DefaultHistoryItem defaultHistoryItem))
            {
                defaultHistoryItem.SetDay(eventTimeLine.CurrentDay)
                    .SetDescription(this.Description);
            }
            
            enemyManager.AddToSpawnList(EnemyID);
        }
    }
}
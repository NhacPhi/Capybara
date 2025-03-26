using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using Tech.Json;
using UnityEngine;

namespace Event_System
{
    public class EventDatabase
    {
        private Dictionary<EventType, List<EventBase>> _eventDict;

        public async UniTask Init(DataService dataService, CancellationToken token = default)
        {
            _eventDict = await dataService.LoadDataAsync<Dictionary<EventType, 
                List<EventBase>>>(AddressConstant.EventData, token);
            dataService.ReleaseData(AddressConstant.EventData);
        }

        public EventBase GetRandomEvent(EventType eventType)
        {
            var listEvent = _eventDict[eventType];
            if(listEvent == null || listEvent.Count == 0) return null;
            return listEvent[Random.Range(0, listEvent.Count)];
        }
    }
}

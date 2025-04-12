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

        public async UniTask Init(CancellationToken token = default)
        {
            var textAsset = await AddressablesManager.Instance.LoadAssetAsync<TextAsset>(
                AddressConstant.EventData, token: token);
            _eventDict = Json.DeserializeObject<Dictionary<EventType, List<EventBase>>>(textAsset.text);
            AddressablesManager.Instance.RemoveAsset(AddressConstant.EventData);
        }

        public EventBase GetRandomEvent(EventType eventType)
        {
            var listEvent = _eventDict[eventType];
            if(listEvent == null || listEvent.Count == 0) return null;
            return listEvent[Random.Range(0, listEvent.Count)];
        }
    }
}

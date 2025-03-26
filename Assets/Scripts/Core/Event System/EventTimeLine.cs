using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Observer;
using Tech.Json;
using UnityEngine;

namespace Event_System
{
    public class EventTimeLine
    {
        private List<DayContent> _timeLine;
        public int CurrentDay { get; private set; }
        public int DayCount => _timeLine.Count;
        
        public async UniTask Init(DataService dataService, CancellationToken token = default)
        {
            _timeLine = await dataService.LoadDataAsync<List<DayContent>>(
                AddressConstant.EventTimeline, token);
        }

        public void OnAfterLoadDone()
        {
            CurrentDay = 1;
            TimelineAction.OnInitTimeline?.Invoke(CurrentDay, DayCount);
        }
        
        public TimeLineType GetDayEvent()
        {
            return _timeLine[CurrentDay - 2].Type;
        }

        public TimeLineType NextDay()
        {
            CurrentDay++;
            return GetDayEvent();
        }
    }

    public enum TimeLineType
    {
        SelectionRandom,
        FightRandom,
        PassiveRandom,
        Boss,
    }

    public class DayContent
    {
        [JsonProperty("Type")]
        public TimeLineType Type { get; private set; } = TimeLineType.PassiveRandom;
    }
}
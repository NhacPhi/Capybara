using System.Collections.Generic;
using System.Threading;
using Core.Entities.Player;
using Cysharp.Threading.Tasks;
using EventType = Event_System.EventType;
using Event_System;
using Observer;
using Tech.Json;
using VContainer;

public class EventManager
{
    [Inject] private DataService _dataService;
    [Inject] public UIManager UIManager { get; private set; }
    [Inject] private GameplayScope _scope;
    private EventDatabase _eventDatabase;
    public EventTimeLine EventTimeLine { get; private set;}
    public EventHistory EventHistory { get; private set;}
    public PlayerCtrl Player { get; private set;}
    
    public void Init(List<UniTask> tasks, CancellationToken token = default)
    {
        _eventDatabase = new();
        EventTimeLine = new();
        
        tasks.Add(_eventDatabase.Init(_dataService, token));
        tasks.Add( EventTimeLine.Init(_dataService, token));

        _ = WaitLoading();
    }

    public void OnAfterLoadDone()
    {
        EventTimeLine.OnAfterLoadDone();
        EventHistory = this.UIManager.GetFirstPanelOfType<GameplayPanel>().EventHistory;
        var evt = _eventDatabase.GetRandomEvent(EventType.Start);
        evt.HandleEvent(this);
    }
    
    private async UniTaskVoid WaitLoading()
    {
        while (_scope.PlayerCtrl == null)
        {
            await UniTask.Yield();
        }
        
        Player = _scope.PlayerCtrl;
    }
    
    public void NextDay()
    {
        var timeLineType = EventTimeLine.NextDay();
        
        EventBase evt = null;
        
        switch (timeLineType)
        {
            case TimeLineType.PassiveRandom:
                evt = _eventDatabase.GetRandomEvent(EventType.Passive);
                break;
            case TimeLineType.SelectionRandom:
                evt = _eventDatabase.GetRandomEvent(EventType.Selection);
                GameAction.OnSelectionEvent?.Invoke();
                break;
            case TimeLineType.FightRandom:
                evt = _eventDatabase.GetRandomEvent(EventType.Fight);
                GameAction.OnStartCombat?.Invoke();
                break;
            case TimeLineType.Boss:
                evt = _eventDatabase.GetRandomEvent(EventType.Boss);
                break;
        }

        evt.HandleEvent(this);
    }
}
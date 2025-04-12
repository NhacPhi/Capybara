using System;
using System.Collections.Generic;
using System.Threading;
using Core.Entities.Player;
using Core.Scope;
using Cysharp.Threading.Tasks;
using EventType = Event_System.EventType;
using Event_System;
using Observer;
using VContainer;

public class EventManager
{
    [Inject] public UIManager UIManager { get; private set; }
    [Inject] private GameplayScope _scope;
    [Inject] private IObjectResolver _objectResolver;
    
    private EventDatabase _eventDatabase;
    public EventTimeLine EventTimeLine { get; private set;}
    public EventHistory EventHistory { get; private set;}
    public PlayerCtrl Player { get; private set;}
    
    public void Init(List<UniTask> tasks, CancellationToken token = default)
    {
        _eventDatabase = new();
        EventTimeLine = new();
        
        tasks.Add(_eventDatabase.Init(token));
        tasks.Add( EventTimeLine.Init(token));

        _ = WaitLoading();
    }

    public void OnAfterLoadDone()
    {
        EventTimeLine.OnAfterLoadDone();
        EventHistory = this.UIManager.GetFirstPanelOfType<GameplayPanel>().EventHistory;
        var evt = _eventDatabase.GetRandomEvent(EventType.Start);
        _objectResolver.Inject(evt);
        evt.HandleEvent();
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
        Action callback = null;
        switch (timeLineType)
        {
            case TimeLineType.PassiveRandom:
                evt = _eventDatabase.GetRandomEvent(EventType.Passive);
                break;
            case TimeLineType.SpecialRandom:
                evt = _eventDatabase.GetRandomEvent(EventType.Special);
                break;
            case TimeLineType.FightRandom:
                evt = _eventDatabase.GetRandomEvent(EventType.Fight);
                callback = GameAction.OnStartCombat;
                break;
            case TimeLineType.Boss:
                evt = _eventDatabase.GetRandomEvent(EventType.Boss);
                break;
        }
        
        _objectResolver.Inject(evt);
        GameAction.OnEvent?.Invoke(evt, callback);
    }
}
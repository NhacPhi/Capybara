using UI;
using UnityEngine;

public class GameplayPanel : PanelBase
{
    [field: SerializeField] public EventHistory EventHistory { get; private set; }
    [field: SerializeField] public EventBtn EventButton { get; private set; }

    private void Reset()
    {
        if (!EventHistory)
        {
            EventHistory = GetComponentInChildren<EventHistory>();
        }
        
        if (!EventButton)
        {
            EventButton = GetComponentInChildren<EventBtn>();
        }
    }

    protected override void OnAwake()
    {
        if (!EventHistory)
        {
            EventHistory = GetComponentInChildren<EventHistory>();
        }
        
        if (!EventButton)
        {
            EventButton = GetComponentInChildren<EventBtn>();
        }
    }
}

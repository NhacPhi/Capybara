using UnityEngine;
using UnityEngine.Events;
using UnlimitedScrollUI;

public class Cell : MonoBehaviour, ICell
{
    public UnityEvent OnEnable;
    public UnityEvent OnDisable;
    
    public void OnBecomeVisible(ScrollerPanelSide side)
    {
        OnEnable?.Invoke();
    }

    public void OnBecomeInvisible(ScrollerPanelSide side)
    {
        OnDisable?.Invoke();
    }
}
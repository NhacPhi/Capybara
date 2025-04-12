using System;
using DG.Tweening;
using Event_System;
using Observer;
using TMPro;
using UnityEngine;
using UnlimitedScrollUI;
using Random = UnityEngine.Random;

public class GachaEventUI : MonoBehaviour
{
    public TextMeshProUGUI TitleText { get; private set; }
    public DynamicScrollCtrl ScrollCtrl { get; private set; }
    [SerializeField] private TextMeshProUGUI _textResult;
    
    private void Awake()
    {
        ScrollCtrl = GetComponentInChildren<DynamicScrollCtrl>();
        TitleText = GetComponentInChildren<TextMeshProUGUI>();
        GameAction.OnEvent += HandleOnEvent;
    }

    private void OnDestroy()
    {
        GameAction.OnEvent -= HandleOnEvent;
    }

    private void Start()
    {
        ScrollCtrl.Scroller.JumpTo(2, JumpToMethod.Center);
    }

    private void HandleOnEvent(EventBase evt, Action callback)
    {
        ScrollCtrl.M_ScrollRect.DOVerticalNormalizedPos(ScrollCtrl.Scroller.IndexToNormalizedPos(
            Random.Range(0, 100)), 1.5f).SetEase(Ease.OutQuint).OnComplete(() =>
        {
            evt.HandleEvent();
            if (!evt.Equals(string.Empty))
            {
                _textResult.text = evt.Description;
            }
            GameAction.OnGachaAnimationDone?.Invoke();
            callback?.Invoke();
        });
    }
}

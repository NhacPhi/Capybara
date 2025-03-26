using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FadeUIComponent))]
public class FadeButton : Button, IHideAndShow
{
    protected FadeUIComponent fadeUI;
    protected const float _fadeDuration = 0.25f;

    protected override void Awake()
    {
        base.Awake();
        fadeUI = GetComponent<FadeUIComponent>();
        OnAwake();
    }

    protected virtual void OnAwake()
    {
        
    }

    public virtual void Hide()
    {
        fadeUI.FadeOut(_fadeDuration);
    }

    public virtual void Show()
    {
        fadeUI.FadeIn(_fadeDuration);
    }
}
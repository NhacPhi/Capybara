using UnityEngine;

[RequireComponent(typeof(FadeUIComponent))]
public abstract class FadePanel : PanelBase
{
    protected const float _fadeDuration = 0.25f;
    protected FadeUIComponent fadeUI;
        
    protected override void OnAwake()
    {
        fadeUI = GetComponent<FadeUIComponent>();
    }

    public override void Hide()
    {
        fadeUI.FadeOut(_fadeDuration);
    }

    public override void Show()
    {
        fadeUI.FadeIn(_fadeDuration);
    }
}
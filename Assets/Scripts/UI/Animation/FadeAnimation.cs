using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeAnimation : AnimationBase
{
    protected CanvasGroup canvasGroup;
    public bool DisableOnComplete;
    public float StartAlpha = 1;
    public float TargetAlpha; 
    protected TweenCallback onComplete;
    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        onComplete = () =>
        {
            if(!DisableOnComplete) return;
            this.gameObject.SetActive(false);
        };
    }

    private void OnEnable()
    {
        canvasGroup.alpha = StartAlpha;
        canvasGroup.DOFade(TargetAlpha, AnimationTime).SetEase(EaseType).OnComplete(onComplete);
    }
}
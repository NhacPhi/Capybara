using DG.Tweening;
using UnityEngine;

public class ScaleAnimation : AnimationBase
{
    public Vector3 BeginScale = Vector3.zero;
    public Vector3 EndScale = Vector3.one;
    
    private void OnEnable()
    {
        transform.localScale = BeginScale;
        transform.DOScale(EndScale, AnimationTime).SetEase(EaseType);
    }
}
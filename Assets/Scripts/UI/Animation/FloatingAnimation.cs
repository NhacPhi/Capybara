using DG.Tweening;

public class FloatingAnimation : AnimationBase
{
    public float Distance = 1f;

    private void OnEnable()
    {
        transform.DOMoveY(transform.position.y + Distance, AnimationTime).SetEase(EaseType);        
    }
}
using Tech.Composite;
using System;

public abstract class AnimationSystemBase : CoreComponent
{
    public abstract void Play(AnimationData data);
    public abstract void RegisterEventAtTime(float normalizedTime, Action onEventTriggered);
    public abstract int OrderInLayer { get; set; }
}

public class AnimationData
{
    public string AnimationName;
    public bool IsLoop;
    public float Transition = 0.25f;
    public int Layer;
    public float TimeScale = 1.0f;
    
    public AnimationData Renew()
    {
        Layer = 0;
        Transition = 0.25f;
        IsLoop = false;
        AnimationName = string.Empty;
        TimeScale = 1.0f;
        
        return this;
    }
}
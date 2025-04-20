using System;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class SpineAnimationCtrl : AnimationSystemBase
{
    protected IAnimationStateComponent animationState;
    protected MeshRenderer meshRenderer;
    protected List<(float normalizedTime, Action callback)> callbacks = new List<(float normalizedTime, Action callback)>();
    
    public override void Play(AnimationData data)
    {
        var trackEntry = animationState.AnimationState.SetAnimation(data.Layer, data.AnimationName, data.IsLoop);
        trackEntry.MixDuration = data.Transition;
        trackEntry.TimeScale = data.TimeScale;
    }

    public override void RegisterEventAtTime(float normalizedTime, Action onEventTriggered)
    {
        normalizedTime = Mathf.Clamp01(normalizedTime);
        callbacks.Add((normalizedTime, onEventTriggered));
    }

    private void Update()
    {
        if(callbacks.Count == 0) return;
        
        var currentTrackEntry = animationState.AnimationState.GetCurrent(0);

        float currentNormalizedTime = Mathf.Repeat(currentTrackEntry.TrackTime / currentTrackEntry.Animation.Duration, 1);
        
        for (int i = callbacks.Count - 1; i >= 0; i--)
        {
            var tuple = callbacks[i];
            
            if (!(currentNormalizedTime >= tuple.normalizedTime)) continue;
            
            tuple.callback?.Invoke();
            callbacks.RemoveAt(i);
        }
    }

    public override int OrderInLayer 
    { 
        get => meshRenderer.sortingOrder; 
        set => meshRenderer.sortingOrder = value; 
    }

    public override void ResetToDefault(int layer = 0, float transition = 0f)
    {
        animationState.AnimationState.SetEmptyAnimation(layer, transition);
    }

    public override void LoadComponent()
    {
        animationState = GetComponent<IAnimationStateComponent>();
        meshRenderer = GetComponent<MeshRenderer>();
    }
}
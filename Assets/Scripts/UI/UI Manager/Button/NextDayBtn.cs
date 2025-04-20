using System;
using Event_System;
using Observer;
using Spine.Unity;
using Tech.Pooling;
using UnityEngine;
using Utilities;

public class NextDayBtn : ButtonBase
{
    [field: Header("Animation")]
    [field: SerializeField] public AnimationSystemBase Anim { get; private set; }
    [SpineAnimation]
    public string JackpotAnim = "Jackpot";
    [SpineAnimation]
    public string MinorWinAnim = "Minor Win";
    [SpineAnimation]
    public string BigWinAnim = "Big Win";
    [SpineAnimation]
    public string BadLuckAnim = "Bad Luck";
    
    [field: Header("FX")]
    [field: SerializeField] public ParticleSystem TapFX { get; private set; }
    [field: SerializeField] public ParticleSystem JackPotFX { get; private set; }
    [field: SerializeField] public ParticleSystem MinorWinFX { get; private set; }
    [field: SerializeField] public ParticleSystem BigWinFX { get; private set; }
    
    private string _animToPlay = string.Empty;
    private ParticleSystem _fxToPlay;
    //Big Win Anim Not Correct X Position
    private const float _xOffsetBigwinAnim = -24f;
    private Action _callbackAnimDone;
    
#if UNITY_EDITOR
    private void Reset()
    {
        var allTransforms = GetComponentsInChildren<Transform>(true);
        
        if(!Anim)
            Anim = GetComponentInChildren<AnimationSystemBase>();
        
        JackPotFX = allTransforms.FindFrist<ParticleSystem>("jackpot");
        MinorWinFX = allTransforms.FindFrist<ParticleSystem>("minorwin");
        BigWinFX = allTransforms.FindFrist<ParticleSystem>("bigwin");
        TapFX = allTransforms.FindFrist<ParticleSystem>("tap");
    }
#endif

    
    public override void OnAwake()
    {
        Btn.onClick.AddListener(() => { EventAction.OnNextDay?.Invoke();});
        
        if(!Anim)
            Anim = GetComponentInChildren<AnimationSystemBase>();
        
        _callbackAnimDone = () =>
        {
            Anim.ResetToDefault();
        };
        
        EventAction.OnBeforeEventHandle += HandleBeforeEventHandle;
        EventAction.OnGachaAnimationDone += HandleGachaDone;
        EventAction.OnEvent += HandleEvent;
    }

    private void OnDestroy()
    {
        EventAction.OnBeforeEventHandle -= HandleBeforeEventHandle;
        EventAction.OnGachaAnimationDone -= HandleGachaDone;
        EventAction.OnEvent -= HandleEvent;
    }
    
    private void HandleBeforeEventHandle(EventBase evt)
    {
        if (_animToPlay.Equals(BigWinAnim))
        {
            var temp = Anim.transform.localPosition;
            temp.x -= _xOffsetBigwinAnim;
            Anim.transform.localPosition = temp;
        }
        
        _animToPlay = string.Empty;
        _fxToPlay = null;
        
        TapFX.Play();
        if(evt is not PassiveEvent evtPassive) return;
            
        switch (evtPassive.Category)
        {
            case PassiveEventCategory.BadLuck:
                _animToPlay = BadLuckAnim;
                return;
            case PassiveEventCategory.MinorWin:
                _fxToPlay = MinorWinFX;
                _animToPlay = MinorWinAnim;
                return;
            case PassiveEventCategory.BigWin:
                _fxToPlay = BigWinFX;
                _animToPlay = BigWinAnim;
                //Only BigWinAnim Not Correct Position So Need Offset
                var temp = Anim.transform.localPosition;
                temp.x += _xOffsetBigwinAnim;
                Anim.transform.localPosition = temp;
                return;
            case PassiveEventCategory.Jackpot:
                _fxToPlay = JackPotFX;
                _animToPlay = JackpotAnim;
                return;
            default:
                return;
        }
    }
    
    private void HandleEvent(EventBase evt, Action callback)
    {
        this.Btn.interactable = false;
    }
    
    private void HandleGachaDone()
    {
        this.Btn.interactable = true;

        PlayAnim();
        PlayFX();
    }

    private void PlayFX()
    {
        if(!_fxToPlay) return;
        
        _fxToPlay.Play();
    }
    
    private void PlayAnim()
    {
        if(string.IsNullOrEmpty(_animToPlay)) return;

        Anim.gameObject.SetActive(true);
        var animationData = GenericPool<AnimationData>.Get().Renew();
        animationData.AnimationName = _animToPlay;
        animationData.Transition = 0;
        Anim.Play(animationData);
        Anim.RegisterEventAtTime(0.95f, _callbackAnimDone);
        GenericPool<AnimationData>.Return(animationData);
    }
}
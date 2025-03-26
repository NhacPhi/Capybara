using UnityEngine;
using VContainer;

[RequireComponent(typeof(CanvasGroup))]
public abstract class PanelBase : MonoBehaviour, IHideAndShow
{
    [Inject] protected IObjectResolver objectResolver;
    public bool HideOnAwake;
    protected CanvasGroup canvasGroup;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        OnAwake();
    }

    protected virtual void OnAwake()
    {
        
    }
    
    public virtual void Hide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public virtual void Show()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }
}
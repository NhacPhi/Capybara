using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class ButtonBase : MonoBehaviour
{
    public Button Btn {get; private set;}
    private void Awake()
    {
        Btn = GetComponent<Button>();
        OnAwake();
    }

    public virtual void OnAwake()
    {
        
    }
}
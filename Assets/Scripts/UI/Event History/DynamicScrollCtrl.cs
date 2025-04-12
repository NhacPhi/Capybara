using UnityEngine;
using UnityEngine.UI;
using UnlimitedScrollUI;

public class DynamicScrollCtrl : MonoBehaviour
{
    public IUnlimitedScroller Scroller { get; protected set; }
    public ScrollRect M_ScrollRect { get; protected set; }
    
    [SerializeField] protected GameObject prefab;
    [SerializeField] protected int prefabCount = 10;
    private void Awake()
    {
        Scroller = GetComponentInChildren<IUnlimitedScroller>();
        M_ScrollRect = GetComponent<ScrollRect>();
        
        Scroller.Generate(prefab, prefabCount, null);
    }
}

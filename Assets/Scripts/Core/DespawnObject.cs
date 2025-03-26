using DG.Tweening;
using UnityEngine;

namespace Core
{
    public class DespawnObject : MonoBehaviour
    {
        [SerializeField] public float DespawnTime = 2f;
        protected Tween despawnTween;

        protected virtual void OnEnable()
        {
            if (despawnTween != null)
            {
                despawnTween.Restart();
                return;
            }
            
            despawnTween = DOVirtual.DelayedCall(DespawnTime, () =>
            {
                this.gameObject.SetActive(false);
            }).SetAutoKill(false);
        }

        protected virtual void OnDestroy()
        {
            despawnTween.Kill();
        }
    }
    
    
}
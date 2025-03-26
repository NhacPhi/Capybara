using UnityEngine;

namespace Tech.Composite
{
    public class CoreComponent : MonoBehaviour
    {
        protected Core core;
        
        protected virtual void Awake()
        {
            core = GetComponent<Core>();
            
            if (!core)
            {
                core = GetComponentInParent<Core>();
            }

            LoadComponent();
        }

        public virtual void LoadComponent()
        {
           
        }
    }
}

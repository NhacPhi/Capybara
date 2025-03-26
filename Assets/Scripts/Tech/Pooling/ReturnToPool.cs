using System;
using UnityEngine;

namespace Tech.Pooling
{
    public class ReturnToPool : MonoBehaviour
    {
        [NonSerialized] public Pool PoolObjects;
        [NonSerialized] public Component RootComponent;
        public void OnDisable()
        {
            if (RootComponent)
            {
                PoolObjects.AddToPool(RootComponent);
                return;
            }
            PoolObjects.AddToPool(gameObject);
        }
    }
}
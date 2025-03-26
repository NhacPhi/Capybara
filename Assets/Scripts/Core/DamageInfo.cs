using UnityEngine;

namespace Core
{
    public struct DamageInfo
    {
        public Transform Source;
        public float Value;

        public DamageInfo(float value, Transform source = null)
        {
            Source = source;
            Value = value;
        }
    }
}
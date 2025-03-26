using System;
using UnityEngine;

namespace Stats.stat
{
    [Serializable]
    public class StatItem
    {
        [SerializeField] private float _baseValue;
        
        public float BaseValue => _baseValue;
    }
}

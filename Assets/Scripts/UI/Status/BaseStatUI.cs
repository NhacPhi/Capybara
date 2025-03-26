using Observer;
using Stats.stat;
using UnityEngine;

namespace UI
{
    public abstract class BaseStatUI : MonoBehaviour
    {
        [field: SerializeField] public StatType StatID { get; protected set; }
        public float LastValue { get; protected set; }
        
        private void Awake()
        {
            InitEvent();
            OnAwake();
        }

        protected virtual void InitEvent()
        {
            switch (StatID)
            {
                case StatType.Atk:
                    PlayerStatusAction.OnAtkChange += HandleValueChange;
                    return;
                case StatType.Def:
                    PlayerStatusAction.OnDefChange += HandleValueChange;
                    return;
                default:
                    return;
            }
        }

        protected virtual void OnAwake(){}
        public abstract void Init(float value);
        public abstract void HandleValueChange(Stat stat);
    }
}
using Observer;
using Stats.M_Attribute;
using Stats.stat;
using Attribute = Stats.M_Attribute.Attribute;

namespace UI
{
    using UnityEngine;
    public abstract class BaseAttributeUI : MonoBehaviour
    {
        [field: SerializeField] public AttributeType AttributeID { get; protected set; }
        public float LastValue { get; protected set; }
        public float LastMaxValue { get; protected set; }
        private void Awake()
        {
            InitEvent();
            OnAwake();
        }
        protected virtual void OnAwake(){}
        public abstract void Init(float value, float maxValue);
        protected virtual void InitEvent()
        {
            switch (AttributeID)
            {
                case AttributeType.Hp:
                    PlayerStatusAction.OnHpChange += HandleValueChange;
                    PlayerStatusAction.OnMaxHpChange += HandleMaxValueChange;
                    return;
                case AttributeType.Exp:
                    PlayerStatusAction.OnExpChange += HandleValueChange;
                    return;
                case AttributeType.Lv:
                    PlayerStatusAction.OnLvChange += HandleValueChange;
                    return;
            }
        }
        protected abstract void HandleValueChange(Attribute attribute);
        protected abstract void HandleMaxValueChange(Stat stat);
    }
}
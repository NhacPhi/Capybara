using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Stats.M_Attribute;
using Stats.stat;
using Tech.Pooling;
using TMPro;
using UnityEngine;
using Attribute = Stats.M_Attribute.Attribute;

namespace UI
{
    public class AttributeTextValue : BaseAttributeUI
    {
        [SerializeField] protected TextMeshProUGUI textValue;
        protected virtual void Reset()
        {
            var allTransform = GetComponentsInChildren<Transform>(true);
            OnReset(allTransform);
        }

        protected virtual void OnReset(Transform[] allTransform)
        {
            if (!textValue)
            {
                textValue = allTransform.FirstOrDefault(x => x.name.ToLower()
                    .Contains("text value")).GetComponentInChildren<TextMeshProUGUI>();
            }

            foreach (AttributeType type in Enum.GetValues(typeof(AttributeType)))
            {
                if (!type.ToString().ToLower().Contains(this.name.ToLower())) continue;
                
                this.AttributeID = type;
                break;
            }
        }
        
        //CultureInfo.InvariantCulture result string ignore culture 76.54 => 76,54 if France
        public override void Init(float value, float maxValue)
        {
            LastValue = value;
            LastMaxValue = maxValue;
            SetTextValue();
        }

        protected override void HandleValueChange(Attribute attribute)
        {
            LastValue = attribute.Value;
            LastMaxValue = attribute.MaxValue;
            SetTextValue();
        }

        protected override void HandleMaxValueChange(Stat stat)
        {
            LastMaxValue = stat.Value;
            SetTextValue();
        }

        protected virtual void SetTextValue()
        {
            var stringBuilder = GenericPool<StringBuilder>.Get().Clear();
            stringBuilder.Append(this.LastValue);
            stringBuilder.Append('/');
            stringBuilder.Append(this.LastMaxValue);
            textValue.text = stringBuilder.ToString();
            GenericPool<StringBuilder>.Return(stringBuilder);
        }
    }
}
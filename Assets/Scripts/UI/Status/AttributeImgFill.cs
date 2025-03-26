using System.Linq;
using System.Text;
using Stats.stat;
using Tech.Pooling;
using UnityEngine;
using UnityEngine.UI;
using Attribute = Stats.M_Attribute.Attribute;

namespace UI
{
    public class AttributeImgFill : AttributeTextValue
    {
        [SerializeField] protected Image fillImageValue;
        protected override void OnReset(Transform[] allTransform)
        {
            base.OnReset(allTransform);
            if (!fillImageValue)
            {
                fillImageValue = allTransform.FirstOrDefault(x => x.name.ToLower()
                    .Contains("fill bar")).GetComponentInChildren<Image>();
            }
        }

        public override void Init(float value, float maxValue)
        {
            this.LastValue = value;
            this.LastMaxValue = maxValue;
            SetUpValue();
        }

        protected override void HandleValueChange(Attribute attribute)
        {
            this.LastValue = attribute.Value;
            this.LastMaxValue = attribute.MaxValue;
            SetUpValue();
        }

        protected override void HandleMaxValueChange(Stat stat)
        {
            this.LastMaxValue = stat.Value;
            SetUpValue();
        }

        private void SetUpValue()
        {
            float ratio = this.LastValue / this.LastMaxValue;
            var stringBuilder = GenericPool<StringBuilder>.Get().Clear();
            stringBuilder.Append(this.LastValue);
            stringBuilder.Append('/');
            stringBuilder.Append(this.LastMaxValue);
            textValue.text = stringBuilder.ToString();
            GenericPool<StringBuilder>.Return(stringBuilder);
            fillImageValue.fillAmount = ratio;
        }
    }
}
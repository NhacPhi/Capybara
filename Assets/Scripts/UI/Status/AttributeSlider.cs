using System.Text;
using Stats.M_Attribute;
using Stats.stat;
using Tech.Pooling;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class AttributeSlider : AttributeTextValue
{
    protected Slider slider;

    public override void Init(float value, float maxValue)
    {
        slider = GetComponentInChildren<Slider>();
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
        stringBuilder.Append(Mathf.CeilToInt(this.LastValue));
        stringBuilder.Append('/');
        stringBuilder.Append(Mathf.CeilToInt(this.LastMaxValue));
        textValue.text = stringBuilder.ToString();
        GenericPool<StringBuilder>.Return(stringBuilder);
        slider.value = ratio;
    }
}
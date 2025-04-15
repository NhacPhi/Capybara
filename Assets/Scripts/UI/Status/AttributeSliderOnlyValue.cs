using System.Globalization;
using UnityEngine;

public class AttributeSliderOnlyValue : AttributeSlider
{
    protected override void SetUpValue()
    {
        textValue.text = Mathf.CeilToInt(this.LastValue).ToString(CultureInfo.InvariantCulture);
        slider.value = this.LastValue / this.LastMaxValue;
    }
}
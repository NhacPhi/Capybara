using System.Linq;
using System.Text;
using Event_System;
using Stats.stat;
using Tech.Pooling;
using TMPro;
using UnityEngine;

public class DefaultHistoryItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _dayText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _valueChangeListText;
    public const string Day = "Day"; 
    
    private void Reset()
    {
        var allTransform = GetComponentsInChildren<Transform>(true);

        if (!_dayText)
        {
            _dayText = allTransform.FirstOrDefault(x => x.name.ToLower().Contains("day"))
                .GetComponentInChildren<TextMeshProUGUI>();
        }

        if (!_descriptionText)
        {
            _descriptionText = allTransform.FirstOrDefault(x => x.name.ToLower()
                .Contains("description")).GetComponentInChildren<TextMeshProUGUI>();
        }

        if (!_valueChangeListText)
        {
            _valueChangeListText = allTransform.FirstOrDefault(x => x.name.ToLower()
                .Contains("value change list")).GetComponentInChildren<TextMeshProUGUI>();;
        }
    }

    private void Awake()
    {
        Refresh();
    }

    public void Refresh()
    {
        _descriptionText.text = string.Empty;
        _dayText.text = string.Empty;
        _valueChangeListText.text = string.Empty;
    }

    public DefaultHistoryItem SetDay(int day)
    {
        var stringBuilder = GenericPool<StringBuilder>.Get().Clear();
        stringBuilder.Append(Day);
        stringBuilder.Append(' ');
        stringBuilder.Append(day);
        _dayText.text = stringBuilder.ToString();
        GenericPool<StringBuilder>.Return(stringBuilder);
        return this;
    }

    public DefaultHistoryItem SetDescription(string description)
    {
        _descriptionText.text = description;
        return this;
    }

    public DefaultHistoryItem SetValueChange(BaseModifyValue[] modifyValues)
    {
        if(modifyValues == null || modifyValues.Length == 0) return this;
        
        var stringBuilder = GenericPool<StringBuilder>.Get().Clear();
        for (var i = 0; i < modifyValues.Length; i++)
        {
            var mod = modifyValues[i];

            CreateValueChangeString(stringBuilder, mod);
            
            if (i != modifyValues.Length - 1)
            {
                stringBuilder.Append(',');    
                stringBuilder.Append(' ');    
            }
            _valueChangeListText.text = stringBuilder.ToString();
            GenericPool<StringBuilder>.Return(stringBuilder);
        }

        return this;
    }
    
    private void CreateValueChangeString(StringBuilder stringBuilder, BaseModifyValue mod)
    {
        stringBuilder.Append(mod.GetNameOfValue());
        
        stringBuilder.Append(' ');

        if (mod.Value >= 0)
        {
            stringBuilder.Append('+');
        }
        
        stringBuilder.Append(mod.Value);
        if (mod.ModType == ModifyType.Percent)
        {
            stringBuilder.Append('%');
        }
    }
}

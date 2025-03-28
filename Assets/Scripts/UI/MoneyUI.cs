using System.Globalization;
using Observer;
using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    private TextMeshProUGUI _textMesh;
    
    private void Awake()
    {
        _textMesh = GetComponentInChildren<TextMeshProUGUI>();
        PlayerStatusAction.OnMoneyChange += HandleMoneyChange;
    }

    private void OnDestroy()
    {
        PlayerStatusAction.OnMoneyChange -= HandleMoneyChange;
    }

    private void HandleMoneyChange(float newValue)
    {
        _textMesh.text = newValue.ToString(CultureInfo.InvariantCulture);
    }
}
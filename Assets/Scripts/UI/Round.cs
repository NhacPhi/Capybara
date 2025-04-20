using System;
using System.Text;
using Observer;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class Round : MonoBehaviour
{
    private TextMeshProUGUI _textMesh;
    public const string ROUND_TEXT = "Round :";
    private int _maxRound;
    
    private void Awake()
    {
        this.gameObject.SetActive(false);
        _textMesh = GetComponent<TextMeshProUGUI>();
        EventAction.OnCombatEnd += HandleCombatEnd;
        EventAction.OnStartCombat += HandleStartCombat;
        EventAction.OnRoundStart += HandleRoundStart;
        EventAction.OnRoundChange += HandleRoundChange;
    }

    private void OnDestroy()
    {
        EventAction.OnStartCombat -= HandleStartCombat;
        EventAction.OnCombatEnd -= HandleCombatEnd;
        EventAction.OnRoundChange -= HandleRoundChange;
    }
    
    private void HandleStartCombat()
    {
        this.gameObject.SetActive(true);
    }

    private void HandleCombatEnd()
    {
        this.gameObject.SetActive(false);
    }
    
    private void HandleRoundStart(int curRound, int maxRound)
    {
        _maxRound = maxRound;
        HandleRoundChange(curRound);
    }

    private void HandleRoundChange(int curRound)
    {
        var stringBuilder = GenericPool<StringBuilder>.Get().Clear();
        stringBuilder.Append(ROUND_TEXT);
        stringBuilder.Append(curRound);
        stringBuilder.Append('/');
        stringBuilder.Append(_maxRound);
        _textMesh.text = stringBuilder.ToString();
        GenericPool<StringBuilder>.Release(stringBuilder);
    }

}

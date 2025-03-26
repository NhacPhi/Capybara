using TMPro;
using UnityEngine;

namespace UI.UI_Manager.Button
{
    public class SelectionBtn : ButtonBase
    {
        [field: SerializeField] public TextMeshProUGUI Title_TMP {get; protected set;}
        [field: SerializeField] public TextMeshProUGUI ValueChange_TMP {get; protected set;}

        private void Reset()
        {
            Title_TMP = transform.Find("Title").GetComponent<TextMeshProUGUI>();
            ValueChange_TMP = transform.Find("Value Change").GetComponent<TextMeshProUGUI>();
        }
    }
}
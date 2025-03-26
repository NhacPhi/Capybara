using System;
using UI.UI_Manager.Button;
using UnityEngine;

namespace UI.UI_Manager
{
    public class SelectionEventGroup : MonoBehaviour
    {
        [field: SerializeField] public SelectionBtn SelectionBtn1 { get; private set; }
        [field: SerializeField] public SelectionBtn SelectionBtn2 { get; private set; }

        private void Reset()
        {
            SelectionBtn1 = transform.GetChild(0).GetComponentInChildren<SelectionBtn>();
            SelectionBtn2 = transform.GetChild(1).GetComponentInChildren<SelectionBtn>();
        }

        private void Awake()
        {
            if (!SelectionBtn1)
            {
                SelectionBtn1 = transform.GetChild(0).GetComponentInChildren<SelectionBtn>();
            }

            if (!SelectionBtn2)
            {
                SelectionBtn2 = transform.GetChild(1).GetComponentInChildren<SelectionBtn>();
            }
        }
    }
}
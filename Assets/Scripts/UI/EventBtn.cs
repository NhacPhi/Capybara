using Core.Scope;
using Observer;
using UI.UI_Manager;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using VContainer;

namespace UI
{
    public class EventBtn : MonoBehaviour
    {
        [field: SerializeField] public Button NextDayBtn { get; private set; }
        [field: SerializeField] public Image IncombatImage { get; private set; }
        [field: SerializeField] public SelectionEventGroup SelectionGroup { get; private set;}
        [Inject] private IPreload _preLoadProgress;
        
#if UNITY_EDITOR
        private void Reset()
        {
            var allTransform = GetComponentsInChildren<Transform>(true);
            
            if (!NextDayBtn)
                NextDayBtn = allTransform.FindFrist<Button>("next day");

            if (!IncombatImage)
                IncombatImage = allTransform.FindFrist<Image>("combat");
            
            SelectionGroup = GetComponentInChildren<SelectionEventGroup>(true);
        }
#endif

        private void Awake()
        {
            LoadComponent();
            RegisterEvent();
        }

        private void OnDestroy()
        {
            DisposeEvent();
        }
        
        private void LoadComponent()
        {
            if (!SelectionGroup)
                SelectionGroup = GetComponentInChildren<SelectionEventGroup>();
                
            if (_preLoadProgress != null)
                _preLoadProgress.OnLoadDone += () => { NextDayBtn.gameObject.SetActive(true); };
        }


        private void RegisterEvent()
        {
            //GameAction.OnSelectionEvent += HandleSelectionEvent;
            EventAction.OnStartCombat += HandleStartCombat;
            EventAction.OnCombatEnd += HandleCombatEnd;
            EventAction.OnSelectionEventDone += HandleSelectionEventDone;
        }


        private void DisposeEvent()
        {
            //GameAction.OnSelectionEvent -= HandleSelectionEvent;
            EventAction.OnStartCombat -= HandleStartCombat;
            EventAction.OnCombatEnd -= HandleCombatEnd;
            EventAction.OnSelectionEventDone -= HandleSelectionEventDone;
           
        }
        
        private void HandleCombatEnd()
        {
            NextDayBtn.gameObject.SetActive(true);
            IncombatImage.gameObject.SetActive(false);
        }

        private void HandleStartCombat()
        {
            IncombatImage.gameObject.SetActive(true);
        }
        
        private void HandleSelectionEventDone()
        {
            SelectionGroup.gameObject.SetActive(false);
        }
    }
}         
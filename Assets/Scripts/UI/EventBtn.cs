using System.Linq;
using Core.Scope;
using Observer;
using UI.UI_Manager;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI
{
    public class EventBtn : MonoBehaviour
    {
        [field: SerializeField] public Button NextDayBtn { get; private set; }
        [field: SerializeField] public Image IncombatImage { get; private set; }
        [field: SerializeField] public SelectionEventGroup SelectionGroup {get; private set;}
        [Inject] private IPreload _preLoadProgress;
        
        private void Reset()
        {
            var allTransform = GetComponentsInChildren<Transform>(true);
            
            if (!NextDayBtn)
            {
                NextDayBtn = allTransform.FirstOrDefault(x => x.name.ToLower().Contains("next day"))
                    .GetComponentInChildren<Button>();
            }

            if (!IncombatImage)
            {
                IncombatImage = allTransform.FirstOrDefault(x => x.name.ToLower().Contains("combat"))
                    .GetComponentInChildren<Image>();
            }
            
            SelectionGroup = GetComponentInChildren<SelectionEventGroup>(true);

            _preLoadProgress.OnLoadDone += () =>
            {
                NextDayBtn.gameObject.SetActive(true);
            };
        }

        private void Awake()
        {
            NextDayBtn.gameObject.SetActive(true);

            if (!SelectionGroup)
            {
                SelectionGroup = GetComponentInChildren<SelectionEventGroup>();
            }

            GameAction.OnSelectionEvent += HandleSelectionEvent;
            GameAction.OnStartCombat += HandleStartCombat;
            GameAction.OnCombatEnd += HandleCombatEnd;
            GameAction.OnSelectionEventDone += HandleSelectionEventDone;
        }


        private void OnDestroy()
        {
            GameAction.OnSelectionEvent -= HandleSelectionEvent;
            GameAction.OnStartCombat -= HandleStartCombat;
            GameAction.OnCombatEnd -= HandleCombatEnd;
            GameAction.OnSelectionEventDone -= HandleSelectionEventDone;
        }
        
        private void HandleCombatEnd()
        {
            NextDayBtn.gameObject.SetActive(true);
            IncombatImage.gameObject.SetActive(false);
        }

        private void HandleStartCombat()
        {
            NextDayBtn.gameObject.SetActive(false);
            IncombatImage.gameObject.SetActive(true);
        }
        
        private void HandleSelectionEventDone()
        {
            SelectionGroup.gameObject.SetActive(false);
        }

        private void HandleSelectionEvent()
        {
            NextDayBtn.gameObject.SetActive(false);
        }
    }
}         
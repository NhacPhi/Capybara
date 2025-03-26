using System.Linq;
using Observer;
using UI.UI_Manager;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EventBtn : MonoBehaviour
    {
        [SerializeField] private Button _nextDayBtn;
        [SerializeField] private Image _incombatImage;
        [field: SerializeField] public SelectionEventGroup SelectionGroup {get; private set;}

        private void Reset()
        {
            var allTransform = GetComponentsInChildren<Transform>(true);
            
            if (!_nextDayBtn)
            {
                _nextDayBtn = allTransform.FirstOrDefault(x => x.name.ToLower().Contains("next day"))
                    .GetComponentInChildren<Button>();
            }

            if (!_incombatImage)
            {
                _incombatImage = allTransform.FirstOrDefault(x => x.name.ToLower().Contains("combat"))
                    .GetComponentInChildren<Image>();
            }
            
            SelectionGroup = GetComponentInChildren<SelectionEventGroup>(true);
        }

        private void Awake()
        {
            _nextDayBtn.gameObject.SetActive(true);

            if (!SelectionGroup)
            {
                SelectionGroup = GetComponentInChildren<SelectionEventGroup>();
            }

            GameAction.OnSelectionEvent += HandleSelectionEvent;
            GameAction.OnPlayerSelect += HandlePlayerSelect;
            GameAction.OnStartCombat += HandleStartCombat;
            GameAction.OnCombatEnd += HandleCombatEnd;
            GameAction.OnSelectionEventDone += HandleSelectionEventDone;
        }


        private void OnDestroy()
        {
            GameAction.OnSelectionEvent -= HandleSelectionEvent;
            GameAction.OnPlayerSelect -= HandlePlayerSelect;
            GameAction.OnStartCombat -= HandleStartCombat;
            GameAction.OnCombatEnd -= HandleCombatEnd;
            GameAction.OnSelectionEventDone -= HandleSelectionEventDone;
        }
        private void HandleCombatEnd()
        {
            _nextDayBtn.gameObject.SetActive(true);
            _incombatImage.gameObject.SetActive(false);
        }

        private void HandleStartCombat()
        {
            _nextDayBtn.gameObject.SetActive(false);
            _incombatImage.gameObject.SetActive(true);
        }
        
        private void HandleSelectionEventDone()
        {
            _nextDayBtn.gameObject.SetActive(true);
            SelectionGroup.gameObject.SetActive(false);
        }


        private void HandlePlayerSelect()
        {
            SelectionGroup.gameObject.SetActive(false);
        }

        private void HandleSelectionEvent()
        {
            _nextDayBtn.gameObject.SetActive(false);
            SelectionGroup.gameObject.SetActive(true);
        }
    }
}         
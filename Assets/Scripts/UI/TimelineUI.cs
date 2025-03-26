using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Observer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TimelineUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _curDayText;
        [SerializeField] private TextMeshProUGUI _nextDayText;
        [SerializeField] private TextMeshProUGUI _dayAmountText;
        [SerializeField] private List<Image> _days;
        [SerializeField] private Color _selectedColor = Color.white;
        private int _currentDay;
        private void Reset()
        {
            var allTransforms = GetComponentsInChildren<Transform>(true);

            if (!_curDayText)
            {
                _curDayText = allTransforms.FirstOrDefault(x => x.name.ToLower()
                    .Contains("current day")).GetComponentInChildren<TextMeshProUGUI>();
            }

            if (!_nextDayText)
            {
                _nextDayText = allTransforms.FirstOrDefault(x => x.name.ToLower()
                    .Contains("next day")).GetComponentInChildren<TextMeshProUGUI>();
            }

            if (!_dayAmountText)
            {
                _dayAmountText = allTransforms.FirstOrDefault(x => x.name.ToLower()
                    .Contains("day amount")).GetComponentInChildren<TextMeshProUGUI>();
            }

            if (_days == null || _days.Count == 0)
            {
                _days = allTransforms.Where(x => x.name.ToLower().Contains("day timeline"))
                    .Select(x => x.GetComponentInChildren<Image>()).ToList();
            }
        }
        private void Awake()
        {
            TimelineAction.OnInitTimeline += HandleInit;
            GameAction.OnNextDay += HandleNextDay;
        }

        private void OnDestroy()
        {
            TimelineAction.OnInitTimeline -= HandleInit;
            GameAction.OnNextDay -= HandleNextDay;
        }
        private void HandleInit(int currentDay, int dayAmount)
        {
            _currentDay = currentDay;
            var nextDay = currentDay + 1;
            _curDayText.text = currentDay.ToString(CultureInfo.InvariantCulture);
            _nextDayText.text = nextDay.ToString(CultureInfo.InvariantCulture);
            _dayAmountText.text = dayAmount.ToString(CultureInfo.InvariantCulture);
            _days[0].color = _selectedColor;
        }
        
        private void HandleNextDay()
        {
            _currentDay++;
            var nextDay = _currentDay + 1;
            _curDayText.text = _currentDay.ToString(CultureInfo.InvariantCulture);
            _nextDayText.text = nextDay.ToString(CultureInfo.InvariantCulture);
        }
    }
}
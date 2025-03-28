using System.Globalization;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Core
{
    public class CombatTextUI : MonoBehaviour
    {
        [field: SerializeField] public TextMeshProUGUI TMP { get; private set; }
        protected FadeUIComponent fadeComponent;
        private void Reset()
        {
            if (!TMP)
            {
                TMP = GetComponentInChildren<TextMeshProUGUI>();
            }
        }

        private void Awake()
        {
            if (!TMP)
            {
                TMP = GetComponentInChildren<TextMeshProUGUI>();
            }

            fadeComponent = GetComponentInChildren<FadeUIComponent>();
        }

        private void OnEnable()
        {
            _ = Wait();
        }
        
        private async UniTaskVoid Wait()
        {
            await UniTask.Yield();
            var fadeTime = 0.5f;
            fadeComponent.FadeOut(fadeTime);
            this.transform.DOMoveY(this.transform.position.y + 1.3f, fadeTime);
        }
        
        public void SetValue(float damage)
        {
            string textDmg = Mathf.CeilToInt(damage).ToString(CultureInfo.InvariantCulture);
            TMP.text = textDmg;
        }
    }
}
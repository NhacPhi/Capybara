using System.Globalization;
using Core.GameLoop;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Core
{
    public class DamagePopupItem : MonoBehaviour
    {
        [field: SerializeField] public TextMeshProUGUI damageText { get; private set; }
        protected FadeUIComponent fadeComponent;
        private void Reset()
        {
            if (!damageText)
            {
                damageText = GetComponentInChildren<TextMeshProUGUI>();
            }
        }

        private void Awake()
        {
            if (!damageText)
            {
                damageText = GetComponentInChildren<TextMeshProUGUI>();
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
        
        public void SetDamage(float damage)
        {
            string textDmg = Mathf.CeilToInt(damage).ToString(CultureInfo.InvariantCulture);
            damageText.text = textDmg;
        }
    }
}
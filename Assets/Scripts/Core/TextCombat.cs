using System;
using Cysharp.Threading.Tasks;
using Tech.Pooling;
using UnityEngine;
using VContainer.Unity;

namespace Core
{
    public class TextCombat : IInitializable, IDisposable
    {
        public const string Address = "Damage Popup";
        private CombatTextUI popupPrefab;
        public static readonly Color DamagePopupColor = Color.white;
        public static readonly Color HealPopupColor = Color.green;

        public void Initialize()
        {
            _ = WaitLoading();
        }

        private async UniTaskVoid WaitLoading()
        {
            while (!AddressablesManager.Instance)
            {
                await UniTask.Yield();
            }

            var prefab = await AddressablesManager.Instance.LoadAssetAsync<GameObject>(Address);
            popupPrefab = prefab.GetComponent<CombatTextUI>();
        }

        public void CreateDamagePopup(float damage, Vector3 position)
        {
            var clone = PoolManager.Instance.SpawnObject(popupPrefab, 
                position, Quaternion.identity);
            clone.SetValue(damage);
            clone.TMP.color = DamagePopupColor;
        }

        public void CreateHealPopup(float heal, Vector3 position)
        {
            var clone = PoolManager.Instance.SpawnObject(popupPrefab, 
                position, Quaternion.identity);
            clone.SetValue(heal);
            clone.TMP.color = HealPopupColor;
        }
        
        public void Dispose()
        {
            if(!AddressablesManager.IsExist) return;
            
            AddressablesManager.Instance.RemoveAsset(Address);
        }
    }
}
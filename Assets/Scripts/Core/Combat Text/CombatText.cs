using System;
using Cysharp.Threading.Tasks;
using Observer;
using Tech.Pooling;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer.Unity;

namespace Core
{
    public class CombatText : IInitializable, IDisposable
    {
        public const string Address = "Damage Popup";
        private CombatTextUI popupPrefab;

        public void Initialize()
        {
            TextPopupAction.DamagePopup += CreateDamagePopup;
            TextPopupAction.HealPopup += CreateHealPopup;
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
            AddressablesManager.Instance.RemoveAsset(Address);
        }

        public void CreateDamagePopup(float damage, Vector3 position)
        {
            var clone = PoolManager.Instance.SpawnObject(popupPrefab, 
                position, Quaternion.identity);
            clone.SetValue(damage);
            clone.TMP.color = Color.white;
        }

        public void CreateHealPopup(float heal, Vector3 position)
        {
            if(heal < 1f && heal > - 1) return;
            
            var clone = PoolManager.Instance.SpawnObject(popupPrefab, 
                position, Quaternion.identity);
            clone.SetValue(heal);
            clone.TMP.color = heal > 0 ? Color.green: Color.red;
        }
        
        public void Dispose()
        {
            TextPopupAction.DamagePopup -= CreateDamagePopup;
            TextPopupAction.HealPopup -= CreateHealPopup;
        }
    }
}
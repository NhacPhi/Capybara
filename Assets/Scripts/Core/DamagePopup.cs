using System;
using Cysharp.Threading.Tasks;
using Tech.Pooling;
using UnityEngine;
using VContainer.Unity;

namespace Core
{
    public class DamagePopup : IInitializable, IDisposable
    {
        public const string Address = "Damage Popup";
        private DamagePopupItem popupPrefab;
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
            popupPrefab = prefab.GetComponent<DamagePopupItem>();
        }

        public void CreatePopup(float damage, Vector3 position)
        {
            var clone = PoolManager.Instance.SpawnObject(popupPrefab, 
                position, Quaternion.identity);
            clone.SetDamage(damage);
        }

        public void Dispose()
        {
            if(!AddressablesManager.IsExist) return;
            
            AddressablesManager.Instance.RemoveAsset(Address);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Tech.Logger;
using UnityEngine;

namespace Tech.Composite
{
    public class Core : MonoBehaviour
    {
        private List<CoreComponent> _coreComponents;
        
        private void Awake()
        {
            Initialize();
        }
        
        public void Initialize()
        {
            if(_coreComponents != null) return;
            _coreComponents = GetComponentsInChildren<CoreComponent>().ToList();
            LoadComponent();
        }

        protected virtual void LoadComponent()
        {
            
        }

        public void AddCoreComponent(CoreComponent component)
        {
            if(_coreComponents.Contains(component)) return;
            
            _coreComponents.Add(component);
        }
        
        public T GetCoreComponent<T>() where T : CoreComponent
        {
            Initialize();
            var comp = _coreComponents.OfType<T>().FirstOrDefault();

            if (comp)
                return comp;

            comp = GetComponentInChildren<T>();

            if (comp)
                return comp;

            //LogCommon.LogWarning($"{typeof(T)} not found on {transform.name}");
            return null;
        }

        public T GetCoreComponent<T>(ref T value) where T : CoreComponent
        {
            value = GetCoreComponent<T>();
            return value;
        }

        public T GetCoreComponent<T>(string objName) where T : CoreComponent
        {
            Initialize();
            foreach (var component in _coreComponents)
            {
                if (component is T tmpComponent && tmpComponent.gameObject.name == objName)
                {
                    return tmpComponent;
                }
            }

            return null;
        }
        public List<T> GetCoreComponents<T>() where T : CoreComponent
        {
            Initialize();
            return _coreComponents.OfType<T>().ToList();
        }

        public void RemoveComponent<T>() where T : CoreComponent
        {
            var componentsList = _coreComponents.OfType<T>().ToList();

            foreach (T component in componentsList)
            {
                Destroy(component);
            }

            _coreComponents.RemoveAll(x => x == null);
        }
    }
}

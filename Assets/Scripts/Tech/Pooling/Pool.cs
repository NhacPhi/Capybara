using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Tech.Pooling
{
    [Serializable]
    public class Pool
    {
        private Stack<Object> _inActiveObjects = new ();
        private Object _baseObject;
		    
        public Pool(Object obj)
        {
            _baseObject = obj;
        }
		    
        public Object GetPool(IObjectResolver objectResolver, Vector3 position = default, Quaternion rotation = default)
        {
            GameObject go = null;
			    
            if (_inActiveObjects.Count > 0)
            {
                var tmp = _inActiveObjects.Pop();
                go = GetInstance(tmp);
                go.transform.position = position;
                go.transform.rotation = rotation;
                go.SetActive(true);
				    
                return tmp;
            }

            if (_baseObject is GameObject gameObject)
            {
                go = objectResolver.Instantiate(gameObject, position, rotation);	
                go.AddComponent<ReturnToPool>().PoolObjects = this;
                return go;
            }

            if (_baseObject is not Component component) return null;
			    
            var cloneComponent = objectResolver.Instantiate(component, position, rotation);
            var returnToPool = cloneComponent.gameObject.AddComponent<ReturnToPool>();
            returnToPool.PoolObjects = this;
            returnToPool.RootComponent = cloneComponent;
            return cloneComponent;
        }

        public void AddToPool(Object obj)
        {
            _inActiveObjects.Push(obj);
        }

        private GameObject GetInstance(Object obj)
        {
            if (obj is Component component)
            {
                return component.gameObject;
            }
			    
			    
            return obj as GameObject;
        }
    }
}


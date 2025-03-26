using System;
using System.Collections.Generic;
using System.Linq;
using Tech.Singleton;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Tech.Pooling
{
	public enum PoolType
	{
		Projectile,
		VFX,
		Enemy,
		None
	}

	public class PoolManager : Singleton<PoolManager>
	{
		[Inject] private IObjectResolver _objectResolver; 
		private readonly Dictionary<Object, Pool> _objectPools = new ();
		private readonly Dictionary<PoolType, Transform> _poolsHolder = new(); 
		
		protected override void Awake()
		{
			base.Awake();
			SetupHolder();
		}

		private void SetupHolder()
		{
			GameObject holder = new GameObject("Pool Holder");
			holder.transform.SetParent(transform);
			var child = new Transform[transform.childCount];
			
			for (int i = 0; i < transform.childCount; i++)
			{
				child[i] = transform.GetChild(i);
			}

			foreach (PoolType pool in Enum.GetValues(typeof(PoolType)))
			{
				if (pool == PoolType.None) continue;
				
				var poolName = pool.ToString();
				
				Transform existTransform = child.FirstOrDefault(x => x.name == poolName);
				
				if (existTransform)
				{
					_poolsHolder.Add(pool, existTransform);
					continue;
				}
				
				GameObject empty = new (poolName);
				empty.transform.SetParent(holder.transform);
				_poolsHolder.Add(pool, empty.transform);
			}
		}

		public GameObject SpawnObject(GameObject objectToSpawn, Vector3 position, Quaternion rotation, PoolType poolType = PoolType.None)
		{
			var spawnableObj = Spawn(objectToSpawn, position, rotation, poolType);
			
			if (poolType != PoolType.None)
			{
				spawnableObj.transform.SetParent(GetPoolParent(poolType).transform);
			}
			
			return spawnableObj;
		}

		public T SpawnObject<T>(T objectToSpawn, Vector3 position, Quaternion rotation,
			PoolType poolType = PoolType.None) where T : Component
		{
			var spawnableObj = Spawn(objectToSpawn, position, rotation, poolType);
			
			if (poolType != PoolType.None)
			{
				spawnableObj.transform.SetParent(GetPoolParent(poolType).transform);
			}
			
			return spawnableObj;
		}

		private T Spawn<T>(T obj, Vector3 position, Quaternion rotation, PoolType poolType = PoolType.None) where T : Object
		{
			if (!_objectPools.ContainsKey(obj))
			{
				_objectPools.Add(obj, new Pool(obj));
			}

			T spawnableObj = _objectPools[obj].GetPool(_objectResolver, position, rotation) as T;
			
			return spawnableObj;
		}
		
		public void ClearPool(bool includePersistent)
		{
			if (!includePersistent) return;
			
			_objectPools.Clear();
		}
		public GameObject GetPoolParent(PoolType poolType)
		{
			return _poolsHolder[poolType].gameObject;
		}
	}
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
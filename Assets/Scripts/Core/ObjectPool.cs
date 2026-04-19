using System.Collections.Generic;
using UnityEngine;

namespace Shinrai.Core
{
    public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private T _objectToPool;
        [SerializeField] private int _poolSize;
        
        private GameObject _parent;
        private List<T> _pooledObjects;
        
        private void Awake()
        {
            CreatePool();
        }

        public T GetPooledObject()
        {
            for (int i = 0; i < _poolSize; i++)
            {
                if (!_pooledObjects[i].gameObject.activeInHierarchy)
                {
                    return _pooledObjects[i];
                }
            }
            return null;
        }
        
        
        
        private void CreatePool()
        {
            if(_objectToPool == null) return;
            
            _parent = new GameObject("ObjectPool-" + _objectToPool.name); 
            
            _pooledObjects = new List<T>();
            for (int i = 0; i < _poolSize; i++)
            {
                T pooledObject = Instantiate(_objectToPool, _parent.transform);
                var currentName = $"{_objectToPool.name}_{i}";
                pooledObject.name = currentName;
                pooledObject.gameObject.SetActive(false);
                _pooledObjects.Add(pooledObject);
            }
        }
    }
}
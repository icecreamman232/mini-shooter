using System.Collections.Generic;
using UnityEngine;

namespace Shinrai.Core
{
    public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private T _objectToPool;
        [SerializeField] private int _poolSize;
        [SerializeField] private GameObject _poolParent;
        
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
            
            _pooledObjects = new List<T>();
            for (int i = 0; i < _poolSize; i++)
            {
                T pooledObject = Instantiate(_objectToPool, null);
                var currentName = _objectToPool.name;
                currentName = currentName.Replace("(Clone)", i.ToString());
                pooledObject.name = currentName;
                pooledObject.gameObject.SetActive(false);
                _pooledObjects.Add(pooledObject);
            }
        }
    }
}
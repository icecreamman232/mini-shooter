using System.Collections;
using System.Collections.Generic;
using Shinrai.Items;
using UnityEngine;

namespace Shinrai.Levels
{
    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField] private ItemPicker _itemPickerPrefab;
        [SerializeField] private float _spawnRange = 1.5f;
        private bool _spawnFinished;
        public bool SpawnFinished => _spawnFinished;

        public void SpawnItems(List<Item> itemList, Vector3 spawnPosition, float range = 0)
        {
            StartCoroutine(OnSpawning(itemList, spawnPosition, range));
        }

        private IEnumerator OnSpawning(List<Item> itemList, Vector3 spawnPosition, float range)
        {
            foreach (var item in itemList)
            {
                var pickerInstance = Instantiate(_itemPickerPrefab, 
                    GetRandomPosition(spawnPosition, range != 0 ? range : _spawnRange),
                    Quaternion.identity);
                pickerInstance.AssignItem(item);
                yield return new WaitForSeconds(0.3f);
            }
            _spawnFinished = true;
        }

        private Vector2 GetRandomPosition(Vector2 center, float range)
        {
            return center + Random.insideUnitCircle * range;
        }
    }
}

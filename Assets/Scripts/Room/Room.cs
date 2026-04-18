using System.Collections;
using System.Collections.Generic;
using Shinrai.Core;
using Shinrai.Items;
using UnityEngine;

namespace Shinrai.Levels
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private Transform[] _enemySpawnPoint;
        [SerializeField] private Transform[] _itemSpawnPoint;
        [SerializeField] private ItemPicker _itemPickerPrefab;
        [SerializeField] private Teleport _teleport;
        [SerializeField] private RoomRender _roomRender;
        
        private int _maxItemNumber = 3;
        private int _maxItemCanPicked = 1;
        private int _itemPicked = 0;
        
        private HashSet<ItemPicker> _spawnedItems;
        
        public Transform PlayerSpawnPoint => _playerSpawnPoint;
        public Transform[] EnemySpawnPoints => _enemySpawnPoint;

        private void Start()
        {
            _spawnedItems = new HashSet<ItemPicker>();
            _teleport.gameObject.SetActive(false);
            _roomRender.Initialize();
        }

        public void SpawnItems()
        {
            //TODO:
            //1. Add shuffle to item list
            //2. Remove picked items from ItemService
            var itemList = ServiceLocator.GetService<ItemService>().GetItemByRarity(Rarity.Common);
            itemList.Shuffle();
            StartCoroutine(OnSpawningItems(itemList));
        }

        private IEnumerator OnSpawningItems(List<Item> itemList)
        {
            var spawnedNumber = 0;
            for (int i = 0; i < _maxItemNumber; i++)
            {
                if (itemList.Count == _spawnedItems.Count) break;
                
                var pickerInstance = Instantiate(_itemPickerPrefab, _itemSpawnPoint[i].position, Quaternion.identity, transform);
                pickerInstance.AssignItem(itemList[i]);
                _spawnedItems.Add(pickerInstance);
                pickerInstance.OnPickedUp += OnItemPickedUp;
                yield return new WaitForSeconds(0.15f);
            }
        }

        private void OnItemPickedUp(ItemPicker picker)
        {
            picker.OnPickedUp -= OnItemPickedUp;
            _spawnedItems.Remove(picker);
            _itemPicked++;
            
            //Player pick up all items or no more item to pick up. Load next room
            if (_itemPicked >= _maxItemCanPicked || _spawnedItems.Count == 0)
            {
                foreach (var itemPicker in _spawnedItems)
                {
                    if (itemPicker != picker)
                    {
                        //Prevent player pick up other items
                        itemPicker.DestroyItem();
                    }
                }
                _teleport.gameObject.SetActive(true);
                EventBus.Emit(new GameEventChanged(GameEvent.ShowTeleport));
            }
        }
    }
}

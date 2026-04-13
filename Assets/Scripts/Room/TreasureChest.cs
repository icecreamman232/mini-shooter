using System.Collections;
using Shinrai.Core;
using Shinrai.Items;
using Shinrai.VFX;
using UnityEngine;

namespace Shinrai.Levels
{
    public class TreasureChest : MonoBehaviour
    {
        [SerializeField] private ItemSpawner _itemSpawner;
        [SerializeField] private SpriteOutline _outline;
        private TreasureChestPromptEvent _treasureChestPromptEvent;
        private bool _isPromptVisible;

        private void Start()
        {
            _treasureChestPromptEvent = new TreasureChestPromptEvent();
            ServiceLocator.GetService<InputService>().InteractInputCallback += OnOpenChest;
        }

        private void OnDestroy()
        {
            ServiceLocator.GetService<InputService>().InteractInputCallback -= OnOpenChest;
        }

        private void OnOpenChest()
        {
            if (!_isPromptVisible) return;

            StartCoroutine(OpeningChestCoroutine());
        }

        private IEnumerator OpeningChestCoroutine()
        {
            var itemList = ServiceLocator.GetService<ItemService>().GetItemByRarity(Rarity.Common);
            _itemSpawner.SpawnItems(itemList, transform.position, 1.5f);
            yield return new WaitUntil(()=> _itemSpawner.SpawnFinished);
            HideChestPrompt();
            Destroy(gameObject);
        }
        

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            ShowChestPrompt();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            HideChestPrompt();
        }

        private void ShowChestPrompt()
        {
            _treasureChestPromptEvent.IsPromptVisible = true;
            _isPromptVisible = true;
            _outline.ShowOutline();
            EventBus.Emit(_treasureChestPromptEvent);
        }
        
        private void HideChestPrompt()
        {
            _treasureChestPromptEvent.IsPromptVisible = false;
            _isPromptVisible = false;
            _outline.HideOutline();
            EventBus.Emit(_treasureChestPromptEvent);
        }
    }
}

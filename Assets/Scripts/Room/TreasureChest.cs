using System;
using Shinrai.Core;
using UnityEngine;

namespace Shinrai.Levels
{
    public class TreasureChest : MonoBehaviour
    {
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
            EventBus.Emit(_treasureChestPromptEvent);
        }
        
        private void HideChestPrompt()
        {
            _treasureChestPromptEvent.IsPromptVisible = false;
            _isPromptVisible = false;
            EventBus.Emit(_treasureChestPromptEvent);
        }
    }
}

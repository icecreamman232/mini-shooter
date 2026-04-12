using Shinrai.Core;
using UnityEngine;

namespace Shinrai.Levels
{
    public class TreasureChest : MonoBehaviour
    {
        private TreasureChestPromptEvent _treasureChestPromptEvent;

        private void Start()
        {
            _treasureChestPromptEvent = new TreasureChestPromptEvent();
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
            EventBus.Emit(_treasureChestPromptEvent);
        }
        
        private void HideChestPrompt()
        {
            _treasureChestPromptEvent.IsPromptVisible = false;
            EventBus.Emit(_treasureChestPromptEvent);
        }
    }
}

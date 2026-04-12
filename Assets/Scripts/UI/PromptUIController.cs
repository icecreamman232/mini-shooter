using Shinrai.Core;
using UnityEngine;

namespace Shinrai.UI
{
    public class PromptUIController : MonoBehaviour
    {
        [SerializeField] private GameObject _treasureChestPrompt;

        private void Start()
        {
            EventBus.Subscribe<TreasureChestPromptEvent>(OnToggleTreasureChestPrompt);
            _treasureChestPrompt.SetActive(false);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<TreasureChestPromptEvent>(OnToggleTreasureChestPrompt);
        }

        private void OnToggleTreasureChestPrompt(TreasureChestPromptEvent treasureChestPromptEvent)
        {
            _treasureChestPrompt.SetActive(treasureChestPromptEvent.IsPromptVisible);
        }
    }
}

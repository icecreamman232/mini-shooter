using Shinrai.Core;
using UnityEngine;

namespace Shinrai.UI
{
    public class PromptUIController : MonoBehaviour
    {
        [SerializeField] private GameObject _pickUpItemPrompt;

        private void Start()
        {
            EventBus.Subscribe<PickupItemPromptEvent>(OnTogglePickupPrompt);
            _pickUpItemPrompt.SetActive(false);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<PickupItemPromptEvent>(OnTogglePickupPrompt);
        }

        private void OnTogglePickupPrompt(PickupItemPromptEvent pickupItemPromptEvent)
        {
            _pickUpItemPrompt.SetActive(pickupItemPromptEvent.IsPromptVisible);
        }
    }
}

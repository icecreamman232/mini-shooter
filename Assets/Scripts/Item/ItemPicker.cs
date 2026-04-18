using System;
using System.Collections;
using Shinrai.Core;
using Shinrai.Data;
using Shinrai.Items;
using Shinrai.VFX;
using TMPro;
using UnityEngine;

namespace Shinrai.Levels
{
    public class ItemPicker : MonoBehaviour
    {
        [SerializeField] private Material _dissolveFXMaterial;
        [SerializeField] private SpriteRenderer _itemIcon;
        [SerializeField] private SpriteOutline _outline;
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _itemDescription;
        [SerializeField] private CanvasGroup _itemUICanvasGroup;
        [SerializeField] private CommonColorData _commonColorData;

        private PickupItemPromptEvent _pickupItemPromptEvent;
        private Item _assignedItem;
        private bool _isSelected;
        private bool _canPickUp = true;
        private MaterialPropertyBlock _propertyBlock;
        
        public Action<ItemPicker> OnPickedUp;

        private void Awake()
        {
            _pickupItemPromptEvent = new PickupItemPromptEvent();
            ServiceLocator.GetService<InputService>().InteractInputCallback += OnPickUp;
            _propertyBlock = new MaterialPropertyBlock();
            _itemUICanvasGroup.alpha = 0;
        }

        private void OnDestroy()
        {
            ServiceLocator.GetService<InputService>().InteractInputCallback -= OnPickUp;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!other.CompareTag("Player")) return;
            OnSelect();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(!other.CompareTag("Player")) return;
            OnDeselect();
        }

        public void DestroyItem()
        {
            _canPickUp = false;
            StartCoroutine(DestroyItemCoroutine(1.5f));
        }

        public void AssignItem(Item item)
        {
            _outline.SetOutlineColor(_commonColorData.GetColorByRarity(item.Definition.Rarity));
            _assignedItem = item;
            _itemIcon.sprite = item.Definition.Icon;
            _itemName.text = item.Definition.DisplayName;
            _itemName.color = _commonColorData.GetColorByRarity(item.Definition.Rarity);
            _itemDescription.text = item.Definition.Description;
        }

        private IEnumerator DestroyItemCoroutine(float duration)
        {
            OnDeselect();
            _itemIcon.sharedMaterial = _dissolveFXMaterial;
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
                float dissolveAmount  = Mathf.SmoothStep(0f, 1f, t);
                // Update property block (works even after material switch)
                _itemIcon.GetPropertyBlock(_propertyBlock);
                _propertyBlock.SetFloat("_DissolveAmount", dissolveAmount);
                _itemIcon.SetPropertyBlock(_propertyBlock);
                yield return null;
            }
            Destroy(gameObject);
        }

        private void OnPickUp()
        {
            if (!_isSelected) return;
            if(!_canPickUp) return;
            var playerController = ServiceLocator.GetService<InGameDataManager>().PlayerController;
            playerController.PlayerInventory.AddItem(_assignedItem);
            OnPickedUp?.Invoke(this);
            ServiceLocator.GetService<ItemService>().RemoveItem(_assignedItem.Definition);
            _pickupItemPromptEvent.IsPromptVisible = false;
            EventBus.Emit(_pickupItemPromptEvent);
            Destroy(gameObject);
        }


        private void OnSelect()
        {
            _outline.ShowOutline();
            _isSelected = true;
            
            //Show item UI
            _itemUICanvasGroup.alpha = 1;
            
            //Show prompt button
            _pickupItemPromptEvent.IsPromptVisible = true;
            EventBus.Emit(_pickupItemPromptEvent);
        }
        
        private void OnDeselect()
        {
            _outline.HideOutline();
            _isSelected = false;
            
            //Hide item UI
            _itemUICanvasGroup.alpha = 0;
            
            //Hide prompt button
            _pickupItemPromptEvent.IsPromptVisible = false;
            EventBus.Emit(_pickupItemPromptEvent);
        }
    } 
}


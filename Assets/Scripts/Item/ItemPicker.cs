using System;
using Shinrai.Core;
using Shinrai.Items;
using Shinrai.VFX;
using UnityEngine;

namespace Shinrai.Levels
{
    public class ItemPicker : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _itemIcon;
        [SerializeField] private SpriteOutline _outline;

        private Item _assignedItem;
        private bool _isSelected;
        
        public Action<ItemPicker> OnPickedUp;

        private void Awake()
        {
            ServiceLocator.GetService<InputService>().InteractInputCallback += OnPickUp;
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

        public void AssignItem(Item item)
        {
            _assignedItem = item;
            _itemIcon.sprite = item.Definition.Icon;
        }

        private void OnPickUp()
        {
            if (!_isSelected) return;
            var playerController = ServiceLocator.GetService<InGameDataManager>().PlayerController;
            playerController.PlayerInventory.AddItem(_assignedItem);
            OnPickedUp?.Invoke(this);
            ServiceLocator.GetService<ItemService>().RemoveItem(_assignedItem.Definition);
            Destroy(gameObject);
        }


        private void OnSelect()
        {
            _outline.ShowOutline();
            _isSelected = true;
        }
        
        private void OnDeselect()
        {
            _outline.HideOutline();
            _isSelected = false;
        }
    } 
}


using Shinrai.Items;
using Shinrai.VFX;
using UnityEngine;

namespace Shinrai.Levels
{
    public class ItemPicker : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _itemIcon;
        [SerializeField] private SpriteOutline _outline;
        
        private bool _isSelected;

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
            _itemIcon.sprite = item.Definition.Icon;
        }

        private void OnPickUp()
        {
            if (!_isSelected) return;
            
        }


        private void OnSelect()
        {
            if (_itemIcon == null) return;
            _outline.ShowOutline();
            _isSelected = true;
        }
        
        private void OnDeselect()
        {
            if (_itemIcon == null) return;
            _outline.HideOutline();
            _isSelected = false;
        }
    } 
}


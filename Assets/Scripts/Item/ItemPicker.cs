using Shinrai.VFX;
using UnityEngine;

namespace Shinrai.Levels
{
    public class ItemPicker : MonoBehaviour
    {
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

        public void AssignItem()
        {
            
        }

        private void OnPickUp()
        {
            if (!_isSelected) return;
            
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


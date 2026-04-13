using System.Collections.Generic;
using Shinrai.Items;
using Shinrai.Modifiers;
using UnityEngine;

namespace Shinrai.Entity
{
    /// <summary>
    /// Player inventory will have no category. It acts as a container that will always add any items that player pickup and apply modifiers to player
    /// </summary>
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private ModifierComponent _modifierComponent;
        [SerializeField] private List<Item> _equippedItems;

        public void Initialize()
        {
            _equippedItems = new List<Item>();
        }
        
        public void AddItem(Item item)
        {
            if (item == null) return;
            _equippedItems.Add(item);
            
            foreach (var modifierInstance in item.ModifierInstances)
            {
                _modifierComponent.AddModifier(modifierInstance);
            }
        }
    }
}

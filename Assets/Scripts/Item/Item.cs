using System;
using System.Collections.Generic;
using Shinrai.Entity;
using Shinrai.Modifiers;
using UnityEngine;

namespace Shinrai.Items
{
    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Legendary,
    }
    
    
    /// <summary>
    /// Item is a runtime class that hold item definition and modifier instances for references. <br/>
    /// Item is used to represent the item that the player has in their inventory. <br/>
    /// <b> ModifierInstances</b> are created from <b>ItemDefinition.ModifierRecords</b> on initialization.
    /// </summary>
    [Serializable]
    public class Item
    {
        [SerializeField] private ItemDefinition _definition;
        [SerializeField] private List<ModifierInstance> _modifierInstances;
        
        public ItemDefinition Definition => _definition;
        public List<ModifierInstance> ModifierInstances => _modifierInstances;

        public Item(ItemDefinition definition)
        {
            _definition = definition;
            _modifierInstances = new List<ModifierInstance>();
            foreach (var modiferRecord in definition.ModifierRecords)
            {
                var modifierInstance = new ModifierInstance(modiferRecord);
                _modifierInstances.Add(modifierInstance);
            }
        }
    }
}

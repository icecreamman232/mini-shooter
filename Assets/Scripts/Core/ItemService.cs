using System;
using System.Collections.Generic;
using System.Linq;
using Shinrai.Items;
using UnityEngine;

namespace Shinrai.Core
{
    public class ItemService : MonoBehaviour, IGameService, IBootStrap
    {
        [SerializeField] private ItemDefinition _testItem;
        [SerializeField] private bool _forceAddTestItem;
        [SerializeField] private List<ItemDefinition> _itemDefinitions;
        
        
        public void Install()
        {
            ServiceLocator.RegisterService(this);
        }

        public void Uninstall()
        {
            ServiceLocator.UnregisterService<ItemService>();
        }

        public void RemoveItem(ItemDefinition itemDefinition)
        {
            _itemDefinitions.Remove(itemDefinition);
        }
        
        public List<Item> GetItemByRarity(Rarity rarity) => GetItemsByFilter(itemDef => itemDef.Rarity == rarity);
        
        private List<Item> GetItemsByFilter(Func<ItemDefinition, bool> filter)
        {
            var matchedItemDefs = _itemDefinitions.Where(filter);
  
            var itemList = new List<Item>();
            foreach (var itemDef in matchedItemDefs)
            {
                var item = new Item(itemDef);
                itemList.Add(item);
            }
            
            //This is for testing purposes
            if (_forceAddTestItem)
            {
                var itemTest = new Item(_testItem);
                itemList[0] = itemTest;
            }
            
            return itemList;
        }
    }
}

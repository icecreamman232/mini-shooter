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
        private List<ItemDefinition> _candidateList = new List<ItemDefinition>();
        
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
            //Player chosen an item. Clear the candidate list so next time player will have chance to pick them
            _itemDefinitions.AddRange(_candidateList);
            _candidateList.Clear();
            _itemDefinitions.Remove(itemDefinition);
        }
        
        private List<Item> GetItemsByRarity(Rarity rarity) => GetItemsByFilter(itemDef => itemDef.Rarity == rarity);

        public Item GetItemByRarity(Rarity rarity)
        {
            var picked = GetItemsByRarity(rarity).PickRandom();
            _candidateList.Add(picked.Definition);
            _itemDefinitions.Remove(picked.Definition);
            return picked;
        }


        private List<Item> GetItemsByFilter(Func<ItemDefinition, bool> filter)
        {
            var matchedItemDefs = _itemDefinitions.Where(filter).ToList();

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

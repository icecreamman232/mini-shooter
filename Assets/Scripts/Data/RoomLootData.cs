using System;
using UnityEngine;

namespace Shinrai.Data
{
    [Serializable]
    public struct LootPercentageByRarity
    {
        public float Common;
        public float Uncommon;
        public float Rare;
        public float Legendary;
    }
    
    
    [CreateAssetMenu(fileName = "Room Loot Data", menuName = "Room Loot Data")]
    public class RoomLootData : ScriptableObject
    {
        [SerializeField] LootPercentageByRarity[] _lootPercentageByRarity;

        public LootPercentageByRarity GetLootPercentByLevel(int level)
        {
            return level >= _lootPercentageByRarity.Length ? _lootPercentageByRarity[^1] : _lootPercentageByRarity[level];
        }
    }

}

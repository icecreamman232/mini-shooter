using Shinrai.Items;
using UnityEngine;

namespace Shinrai.Data
{
    [CreateAssetMenu(fileName = "Common Color Data", menuName = "CommonColorData")]
    public class CommonColorData : ScriptableObject
    {
        [SerializeField] private Color _rarityCommonColor;
        [SerializeField] private Color _rarityUncommonColor;
        [SerializeField] private Color _rarityRareColor;
        [SerializeField] private Color _rarityLegendaryColor;
        
        public Color RarityCommonColor => _rarityCommonColor;
        public Color RarityUncommonColor => _rarityUncommonColor;
        public Color RarityRareColor => _rarityRareColor;
        public Color RarityLegendaryColor => _rarityLegendaryColor;
        
        public Color GetColorByRarity(Rarity rarity) => rarity switch
        {
            Rarity.Common => RarityCommonColor,
            Rarity.Uncommon => RarityUncommonColor,
            Rarity.Rare => RarityRareColor,
            Rarity.Legendary => RarityLegendaryColor,
            _ => throw new System.ArgumentOutOfRangeException(nameof(rarity), rarity, null)
        };
    }
}

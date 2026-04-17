using System.Collections.Generic;
using Shinrai.Modifiers;
using UnityEngine;

namespace Shinrai.Items
{
    [CreateAssetMenu(fileName = "Item Definition", menuName = "Items/Definition")]
    public class ItemDefinition : ScriptableObject
    {
        [SerializeField] private Rarity _rarity;
        [SerializeField] private string _displayName;
        [SerializeField] [TextArea] private string _description;
        [SerializeField] private Sprite _icon;
        [SerializeField] private List<ModifierRecord> _modifierRecords;
        public List<ModifierRecord> ModifierRecords => _modifierRecords;
        public Rarity Rarity => _rarity;
        public string DisplayName => _displayName;
        public string Description => _description;
        public Sprite Icon => _icon;
    }
}

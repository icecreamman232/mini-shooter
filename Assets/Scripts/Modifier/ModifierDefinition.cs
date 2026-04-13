using UnityEngine;

namespace Shinrai.Modifiers
{
    [CreateAssetMenu(fileName = "Modifier Definition", menuName = "Modifiers/Definition")]
    public class ModifierDefinition : ScriptableObject
    {
        /// <summary>
        /// Which stat is being modified
        /// </summary>
        public StatTarget StatTarget;
        /// <summary>
        /// How the stat is modified
        /// </summary>
        public ModifierOperationType OperationType;
    }
}

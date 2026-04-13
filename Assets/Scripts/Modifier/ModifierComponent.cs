using System.Collections.Generic;
using UnityEngine;

namespace Shinrai.Modifiers
{
    public class ModifierComponent : MonoBehaviour
    {
        private Dictionary<StatTarget, List<ModifierInstance>> _modifiers = new();
        
        /// <summary>
        /// Use this to track which stat targets need to be recalculated
        /// </summary>
        private Dictionary<StatTarget, bool> _markDirtyStatTargets = new();
        
        public void AddModifier(ModifierInstance modifierInstance)
        {
            if (_modifiers.ContainsKey(modifierInstance.Definition.StatTarget))
            {
                _modifiers[modifierInstance.Definition.StatTarget].Add(modifierInstance);
            }
            else
            {
                _modifiers.Add(modifierInstance.Definition.StatTarget, new List<ModifierInstance> {modifierInstance});
            }
            
            //Save the stat target to be recalculated
            _markDirtyStatTargets[modifierInstance.Definition.StatTarget] = true;
        }

        private void RecalculateStats()
        {
            
        }
    }
}

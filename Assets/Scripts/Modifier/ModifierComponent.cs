using System.Collections.Generic;
using UnityEngine;

namespace Shinrai.Modifiers
{
    public class ModifierComponent : MonoBehaviour
    {
        [SerializeField] private StatComponent _statComponent;
        private Dictionary<StatTarget, List<ModifierInstance>> _modifiers = new();
        
        /// <summary>
        /// Use this to track which stat targets need to be recalculated
        /// </summary>
        private Dictionary<StatTarget, bool> _markDirtyStatTargets = new();
        private List<IModifierOperationStrategy> _modifierOperationStrategies;
        private Dictionary<ModifierOperationType, List<float>> _passScratch = new();

        private void Start()
        {
            //This order of operations must be respected.
            //Changing the order will change the outcome result, likely breaking the game.
            _modifierOperationStrategies = new()
            {
                new FlatModifierOperationStrategy(),
                new AddPercentModifierOperationStrategy(),
                new MultPercentModifierOperationStrategy(),
            };
            
            
            _passScratch[ModifierOperationType.Flat] = new List<float>();
            _passScratch[ModifierOperationType.AddPercent] = new List<float>();
            _passScratch[ModifierOperationType.MultiplyPercent] = new List<float>();
        }

        public void AddModifier(ModifierInstance modifierInstance)
        {
            _markDirtyStatTargets.Clear();
            
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
            
            RecalculateStats();
        }

        private void RecalculateStats()
        {
            foreach (var dirtyStatTarget in _markDirtyStatTargets)
            {
                var modifiers = _modifiers[dirtyStatTarget.Key];
                foreach (var list in _passScratch.Values)
                    list.Clear();
                
                
                foreach (var modifier in modifiers)
                {
                    var operation = modifier.Definition.OperationType;
                    _passScratch[operation].Add(modifier.RolledValue);
                }


                var finalValue = _statComponent.GetBase(dirtyStatTarget.Key);
                
                foreach (var operationStrat in _modifierOperationStrategies)
                {
                    finalValue = operationStrat.Apply(finalValue, _passScratch[operationStrat.OperationType]);
                }
                
                _statComponent.SetFinal(dirtyStatTarget.Key, finalValue);
            }
        }
    }
}

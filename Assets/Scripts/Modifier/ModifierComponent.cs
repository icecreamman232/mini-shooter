using System.Collections;
using System.Collections.Generic;
using Shinrai.Core;
using UnityEngine;

namespace Shinrai.Modifiers
{
    public class ModifierComponent : MonoBehaviour
    {
        [SerializeField] private StatComponent _statComponent;
        private Dictionary<StatTarget, List<ModifierInstance>> _modifiers = new();
        private Dictionary<StatTarget, List<ModifierInstance>> _conditionalModifiers = new();
        
        private bool _recalculationScheduled;
        /// <summary>
        /// Use this to track which stat targets need to be recalculated
        /// </summary>
        private Dictionary<StatTarget, bool> _markDirtyStatTargets = new();
        private List<IModifierOperationStrategy> _modifierOperationStrategies;
        private Dictionary<ModifierOperationType, List<float>> _passScratch = new();
        private ConditionEvaluator _conditionEvaluator;
        
        
        private void Start()
        {
            EventBus.Subscribe<ExternalStateChangeEvent>(OnStatChangedExternally);
            
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
            
            _conditionEvaluator = new ConditionEvaluator();
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<ExternalStateChangeEvent>(OnStatChangedExternally);
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

            if (modifierInstance.CompiledCondition != null)
            {
                if (_conditionalModifiers.ContainsKey(modifierInstance.Definition.StatTarget))
                {
                    _conditionalModifiers[modifierInstance.Definition.StatTarget].Add(modifierInstance);
                }
                else
                {
                    _conditionalModifiers.Add(modifierInstance.Definition.StatTarget, new List<ModifierInstance> {modifierInstance});
                }
            }

            if (IsConditionMet(modifierInstance))
            {
                //Save the stat target to be recalculated
                _markDirtyStatTargets[modifierInstance.Definition.StatTarget] = true;
            }
            
            CalculateStats();
        }

        private bool IsConditionMet(ModifierInstance modifierInstance)
        {
            if (modifierInstance.CompiledCondition == null) return true;
            return _conditionEvaluator.IsMet(modifierInstance.CompiledCondition, _statComponent);
        }
        
        private void OnStatChangedExternally(ExternalStateChangeEvent eventArgs)
        {
            _markDirtyStatTargets.Clear();
            if (_conditionalModifiers.ContainsKey(eventArgs.StatTarget))
            {
                _markDirtyStatTargets[eventArgs.StatTarget] = true;
            }

            if (!_recalculationScheduled)
            {
                _recalculationScheduled = true;
                StartCoroutine(DeferredRecalculation());
            }
        }

        private IEnumerator DeferredRecalculation()
        {
            yield return null;
            CalculateStats();
            _recalculationScheduled = false;
        }

        private void CalculateStats()
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

                Debug.Log($"Calculated final value for {dirtyStatTarget.Key}: {finalValue}");
                _statComponent.SetFinal(dirtyStatTarget.Key, finalValue);
            }
        }
    }
}

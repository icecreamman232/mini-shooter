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
        private Dictionary<StatTarget, List<ModifierInstance>> _conditionDependencyIndex = new();

        private bool _recalculationScheduled = false;
        /// <summary>
        /// Use this to track which stat targets need to be recalculated
        /// </summary>
        private Dictionary<StatTarget, bool> _markDirtyStatTargets = new();
        private Dictionary<StatTarget, bool> _markDirtyStatForExternal = new();
        private List<IModifierOperationStrategy> _modifierOperationStrategies;
        private Dictionary<ModifierOperationType, List<float>> _passScratch = new();
        private ConditionEvaluator _conditionEvaluator;

        private readonly Dictionary<ModifierInstance, bool> _lastConditionState = new();

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

            Util.AddToDictionaryList(_modifiers, modifierInstance.Definition.StatTarget, modifierInstance);

            if (modifierInstance.CompiledCondition != null)
            {
                Util.AddToDictionaryList(_conditionalModifiers, modifierInstance.Definition.StatTarget, modifierInstance);

                foreach (var dependency in modifierInstance.GetConditionDependencies())
                {
                    Util.AddToDictionaryList(_conditionDependencyIndex, dependency, modifierInstance);
                }

                _lastConditionState[modifierInstance] = IsConditionMet(modifierInstance);
                if (_lastConditionState[modifierInstance])
                {
                    _markDirtyStatTargets[modifierInstance.Definition.StatTarget] = true;
                }
            }
            else
            {
                _markDirtyStatTargets[modifierInstance.Definition.StatTarget] = true;
            }

            CalculateStats(_markDirtyStatTargets.Keys);
        }

        private bool IsConditionMet(ModifierInstance modifierInstance)
        {
            if (modifierInstance.CompiledCondition == null) return true;
            return _conditionEvaluator.IsMet(modifierInstance.CompiledCondition, _statComponent);
        }

        private void OnStatChangedExternally(ExternalStateChangeEvent eventArgs)
        {
            _markDirtyStatForExternal.Clear();

            if (_conditionDependencyIndex.ContainsKey(eventArgs.StatTarget))
            {
                foreach (var modifier in _conditionDependencyIndex[eventArgs.StatTarget])
                {
                    bool newConditionState = IsConditionMet(modifier);

                    if (_lastConditionState.TryGetValue(modifier, out bool oldConditionState) && oldConditionState != newConditionState)
                    {
                        _lastConditionState[modifier] = newConditionState;
                        _markDirtyStatForExternal[modifier.Definition.StatTarget] = true;
                    }
                }
            }

            if (!_recalculationScheduled && _markDirtyStatForExternal.Count > 0)
            {
                _recalculationScheduled = true;
                StartCoroutine(DeferredRecalculation());
            }
        }

        private IEnumerator DeferredRecalculation()
        {
            yield return null;
            CalculateStats(_markDirtyStatForExternal.Keys);
            _recalculationScheduled = false;
        }

        private void CalculateStats(IEnumerable<StatTarget> markDirtyStatTarget)
        {
            foreach (var dirtyStatTarget in markDirtyStatTarget)
            {
                if (!_modifiers.ContainsKey(dirtyStatTarget))
                    continue;

                var modifiers = _modifiers[dirtyStatTarget];
                foreach (var list in _passScratch.Values)
                    list.Clear();

                foreach (var modifier in modifiers)
                {
                    bool isActive = true;

                    if (modifier.CompiledCondition != null)
                    {
                        if (!_lastConditionState.TryGetValue(modifier, out isActive))
                            isActive = IsConditionMet(modifier);
                    }

                    if (modifier.CompiledCondition == null || isActive)
                    {
                        var operation = modifier.Definition.OperationType;
                        _passScratch[operation].Add(modifier.RolledValue);
                    }
                }

                var finalValue = _statComponent.GetBase(dirtyStatTarget);

                foreach (var operationStrat in _modifierOperationStrategies)
                {
                    finalValue = operationStrat.Apply(finalValue, _passScratch[operationStrat.OperationType]);
                }
                
                _statComponent.SetFinal(dirtyStatTarget, finalValue);
            }
        }
    }
}

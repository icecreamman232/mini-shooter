using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shinrai.Modifiers
{
    public enum StatTarget
    {
        MaxHP,
        CurrentHP,
        MinDamage,
        MaxDamage,
        MoveSpeed,
        FireRate,
        NumberOfShot,
        ProjectileSize,
        ProjectileSpeed,
    }
    
    public enum ModifierOperationType { Flat, AddPercent, MultiplyPercent }
    
    public enum ValueSourceMode {Flat, Range, Formula }

    [Serializable]
    public class ValueSource
    {
        public ValueSourceMode Mode;
        public float FlatValue;
        public float MinValue;
        public float MaxValue;
        public string Formula;
    }

    /// <summary>
    /// This class is used to store definite of a modifier and its source type.
    /// Note: The source type only provides a way to define the value of the modifier, not the rolled value
    /// This will be used by item to be friendly to designer for quickly editing in inspector
    /// </summary>
    [Serializable]
    public class ModifierRecord
    {
        public ModifierDefinition Definition;
        public float Duration; // 0 means permanent
        public ValueSource Source;
        [SerializeReference]
        public List<ConditionNode> Conditions;
        
        public IConditionSpecification BuildCondition()
        {
            if (Conditions == null || Conditions.Count == 0) return AlwaysTrueSpecification.Instance;
            
            IConditionSpecification result = Conditions[0].ToSpec();
            for (int i= 1 ;i < Conditions.Count; i++)
            {
                result = result.And(Conditions[i].ToSpec());
            }
            return result;
        }

        public string GetConditionPreview()
        {
            if(Conditions == null || Conditions.Count == 0) return "Always Active";
            
            var parts = new List<string>();
            foreach (var condition in Conditions)
            {
                parts.Add(condition.GetSummary());
            }
            return string.Join(" AND ", parts);
        }
    }
    
    
    /// <summary>
    /// This class is used to store the rolled value of a modifier with its definition.
    /// This is the data that other systems use to apply the modifier to an entity.
    /// </summary>
    [Serializable]
    public class ModifierInstance
    {
        public ModifierDefinition Definition;
        public float RolledValue;
        public readonly float MaxDuration;
        public float Duration;
        public float AccumulatedValue = 0;
        public IConditionSpecification CompiledCondition;

        public ModifierInstance(ModifierRecord record)
        {
            Definition = record.Definition;
            MaxDuration = record.Duration;
            Duration = 0;
            RolledValue = record.Source.Mode switch
            {
                ValueSourceMode.Flat => record.Source.FlatValue,
                ValueSourceMode.Range => UnityEngine.Random.Range(record.Source.MinValue, record.Source.MaxValue),
                ValueSourceMode.Formula => UnityEngine.Random.Range(record.Source.MinValue, record.Source.MaxValue),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            CompiledCondition = record.BuildCondition();
        }
        
        public IEnumerable<StatTarget> GetConditionDependencies()
        {
            if(CompiledCondition == null) yield break;
            foreach (var dependency in CompiledCondition.GetDependentStats())
            {
                yield return dependency;
            }
        }
    }
}

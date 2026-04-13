
using System.Collections.Generic;

namespace Shinrai.Modifiers
{
    public interface IModifierOperationStrategy
    {
        ModifierOperationType OperationType { get; }
        float Apply(float current, IReadOnlyList<float> values);
    }
    
    //==============================================================================
    //Add all flat values together, add to the base
    //==============================================================================
    public sealed class FlatModifierOperationStrategy : IModifierOperationStrategy
    {
        public ModifierOperationType OperationType => ModifierOperationType.Flat;
        public float Apply(float current, IReadOnlyList<float> values)
        {
            float sum = 0f;
            for(int i = 0; i < values.Count; i++)
            {
                sum += values[i];
            }
            return current + sum;
        }
    }
    
    //==============================================================================
    //Pool all additive percentages, apply as a single multiplier
    //Eg: (+20%) + (+30%) = x1.5, not 1.2 x 1.3
    //==============================================================================
    public sealed class AddPercentModifierOperationStrategy : IModifierOperationStrategy
    {
        public ModifierOperationType OperationType => ModifierOperationType.AddPercent;
        public float Apply(float current, IReadOnlyList<float> values)
        {
            float totalPercent = 0f;
            for (int i = 0; i < values.Count; i++)
            {
                totalPercent += values[i];
            }

            return current * (1 + totalPercent);
        }
    }
    
    //==============================================================================
    // Each modifier multiplies independently — this is what makes
    // PercentMore rare and powerful relative to PercentAdd.
    // (×30% More) AND (×30% More) = ×1.3 × ×1.3 = ×1.69, not ×1.60
    //==============================================================================
    
    public sealed class MultPercentModifierOperationStrategy : IModifierOperationStrategy
    {
        public ModifierOperationType OperationType => ModifierOperationType.MultiplyPercent;
        public float Apply(float current, IReadOnlyList<float> values)
        {
            float result = current;
            for (int i = 0; i < values.Count; i++)
            {
                result *= (1f + values[i]);
            }

            return result;
        }
    }
    
}

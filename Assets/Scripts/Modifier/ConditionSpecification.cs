using System.Collections.Generic;


namespace Shinrai.Modifiers
{
    //===================================================================
    // This script contains all the condition logic for modifiers
    // Each one answer exactly one question about the game state
    // Make the condition is general and simple as much as possible since we can combine them to create more complex conditions
    //===================================================================
    
    
    public sealed class EvaluationContext
    {
        private Dictionary<StatTarget, float> _stats;
        
        public EvaluationContext(Dictionary<StatTarget, float> stats) { _stats = stats; }
        
        public float GetStat(StatTarget statTarget) { return _stats[statTarget]; }
        
        public bool HasStat(StatTarget statTarget) { return _stats.ContainsKey(statTarget); }
    }
    

    public interface IConditionSpecification
    {
        bool IsMet(EvaluationContext context);
    }
    
    
    
    //====== COMPOSITE CONDITION SPECIFICATIONS ======//
    
    public sealed class AndConditionSpecification : IConditionSpecification
    {
        private IConditionSpecification _left;
        private IConditionSpecification _right;
        
        public AndConditionSpecification(IConditionSpecification left, IConditionSpecification right)
        {
            _left = left;
            _right = right;
        }
        
        public bool IsMet(EvaluationContext context) { return _left.IsMet(context) && _right.IsMet(context); }
    }

    public sealed class OrConditionSpecification : IConditionSpecification
    {
        private IConditionSpecification _left;
        private IConditionSpecification _right;

        public OrConditionSpecification(IConditionSpecification left, IConditionSpecification right)
        {
            _left = left;
            _right = right;
        }
        
        public bool IsMet(EvaluationContext context)
        {
            return _left.IsMet(context) || _right.IsMet(context);
        }
    }
    
    public sealed class NotConditionSpecification : IConditionSpecification
    {
        private IConditionSpecification _condition;


        public NotConditionSpecification(IConditionSpecification condition)
        {
            _condition = condition;
        }
        
        public bool IsMet(EvaluationContext context)
        {
            return !_condition.IsMet(context);
        }
    }
    
    //====== SIMPLE CONDITION SPECIFICATIONS ======//

    public sealed class AlwaysTrueSpecification : IConditionSpecification
    {
        public static readonly AlwaysTrueSpecification Instance = new AlwaysTrueSpecification();
        private AlwaysTrueSpecification() { }
        public bool IsMet(EvaluationContext ctx) => true;
    }
    
    
    
    public sealed class HealthBelowThresholdSpec : IConditionSpecification
    {
        private float _threshold;
        
        public HealthBelowThresholdSpec(float threshold) { _threshold = threshold; }
        
        public bool IsMet(EvaluationContext context)
        {
            var maxHealth = context.GetStat(StatTarget.MaxHP);
            if (maxHealth == 0) return false;
            var ratio = context.GetStat(StatTarget.CurrentHP) / maxHealth;
            return ratio < _threshold;
        }
    }

    public sealed class HealthAboveThresholdSpec : IConditionSpecification
    {
        private float _threshold;
        public HealthAboveThresholdSpec(float threshold) { _threshold = threshold; }
        
        public bool IsMet(EvaluationContext context)
        {
            var maxHealth = context.GetStat(StatTarget.MaxHP);
            if (maxHealth == 0) return false;
            var ratio = context.GetStat(StatTarget.CurrentHP) / maxHealth;
            return ratio > _threshold;
        }
    }
    
    
    // ==================================================================
    // FLUENT BUILDER — composes specs without newing up composites manually
    //
    // Usage:
    //   var condition = Spec.HasBuff("Shield")
    //                       .And(Spec.HealthBelow(0.5f))
    //                       .Or(Spec.WorldAbove("FloorNumber", 4f));
    //
    // Each method returns IConditionSpecification, so chains are arbitrarily deep.
    // ==================================================================


    public static class Spec
    {
        public static IConditionSpecification And(this IConditionSpecification left, IConditionSpecification right) => new AndConditionSpecification(left, right);
        public static IConditionSpecification Or(this IConditionSpecification left, IConditionSpecification right) => new OrConditionSpecification(left, right);
        public static IConditionSpecification Not(this IConditionSpecification condition) => new NotConditionSpecification(condition);
        
        public static IConditionSpecification HealthAbove(float threshold) => new HealthAboveThresholdSpec(threshold);
        public static IConditionSpecification HealthBelow(float threshold) => new HealthBelowThresholdSpec(threshold);
        public static IConditionSpecification Always => AlwaysTrueSpecification.Instance;
    }
    
}

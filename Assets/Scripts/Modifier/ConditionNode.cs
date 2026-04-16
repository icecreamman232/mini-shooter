
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shinrai.Modifiers
{
    [Serializable]
    public abstract class ConditionNode
    {
        public abstract IConditionSpecification ToSpec();
        public abstract string GetSummary();
    }

    [Serializable]
    public class HealthAboveNode : ConditionNode
    {
        [Range(0f, 1f)]
        [Tooltip("Health threshold (0-1). Eg: 0.3 = 30%")]
        public float Threshold;

        public override IConditionSpecification ToSpec() => Spec.HealthAbove(Threshold);

        public override string GetSummary() => $"HP > {Threshold * 100f:0}%";
    }
    
    [Serializable]
    public class HealthBelowNode : ConditionNode
    {
        [Range(0f, 1f)]
        [Tooltip("Health threshold (0-1). Eg: 0.3 = 30%")]
        public float Threshold;

        public override IConditionSpecification ToSpec() => Spec.HealthBelow(Threshold); 

        public override string GetSummary() => $"HP < {Threshold * 100f:0}%";
    }
    
    public enum CompositeOperator { And, Or }

    [Serializable]
    public class CompositeConditionNode : ConditionNode
    {
        public CompositeOperator @operator = CompositeOperator.And;
        
        [SerializeReference]
        public List<ConditionNode> children = new List<ConditionNode>();
        
        public override IConditionSpecification ToSpec()
        {
            if (children == null || children.Count == 0) return AlwaysTrueSpecification.Instance;

            IConditionSpecification result = children[0].ToSpec();
            for (int i = 0; i < children.Count; i++)
            {
                result = @operator == CompositeOperator.And
                    ? result.And(children[i].ToSpec())
                    : result.Or(children[i].ToSpec());
            }
            return result;
        }

        public override string GetSummary()
        {
            if (children == null || children.Count == 0) return "(empty)";
            var op = @operator == CompositeOperator.And ? "AND" : "OR";
            var parts = new List<string>();
            foreach (var child in children)
                parts.Add(child.GetSummary());
            return string.Join(op, parts);
        }
    }
    
}

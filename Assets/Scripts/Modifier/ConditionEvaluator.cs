using System.Collections.Generic;

namespace Shinrai.Modifiers
{
    public class ConditionEvaluator
    {
        public bool IsMet(IConditionSpecification condition, StatComponent statComponent)
        {
            if (condition == null || condition == AlwaysTrueSpecification.Instance)
                return true;
            
            EvaluationContext context = BuildContext(statComponent);
            return condition.IsMet(context);
        }
        
        private EvaluationContext BuildContext(StatComponent stats)
        {
            // Snapshot all stat finals into a plain dictionary
            var statSnapshot = new Dictionary<StatTarget, float>();
            foreach (StatTarget stat in stats.AllStats)
                statSnapshot[stat] = stats.GetFinal(stat);
 
            return new EvaluationContext(statSnapshot);
        }
    }
}

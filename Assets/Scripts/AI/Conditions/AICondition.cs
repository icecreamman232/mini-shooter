using Shinrai.Entity;
using UnityEngine;

namespace Shinrai.AI
{
    public class AICondition : MonoBehaviour
    {
        public virtual bool IsConditionMet(EnemyController controller)
        {
            return true;
        }
    }
}

using Shinrai.Entity;
using UnityEngine;

namespace Shinrai.AI
{
    public class AIBehavior : ScriptableObject
    {
        public virtual void Initialize(EnemyController controller)
        {
            
        }

        public virtual void Clean()
        {
            
        }
        
        public virtual void OnEnterState(EnemyController controller)
        {
        }

        public virtual void OnUpdate(EnemyController controller)
        {
            
        }
        
        public virtual void OnExitState(EnemyController controller)
        {
            
        }
    }
}

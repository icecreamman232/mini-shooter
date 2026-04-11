using Shinrai.Entity;
using UnityEngine;

namespace Shinrai.AI
{
    public class AIBehavior : ScriptableObject
    {
        protected EnemyController _controller;
        
        public virtual void Initialize(EnemyController controller)
        {
            _controller = controller;
        }

        public virtual void Clean()
        {
            _controller = null;
        }
        
        public virtual void OnEnterState()
        {
        }

        public virtual void OnUpdate()
        {
            
        }
        
        public virtual void OnExitState()
        {
            
        }
    }
}

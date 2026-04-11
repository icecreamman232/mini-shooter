using Shinrai.AI;
using UnityEngine;

namespace Shinrai.Entity
{
    public class EnemyController : EntityController
    {
        [SerializeField] private EnemyMovement _movement;
        [SerializeField] private EnemyHealth _health;
        [SerializeField] private EnemyAIController _aiController;
        
        
        public EnemyMovement Movement => _movement;
        public EnemyHealth Health => _health;
        
        

        private void Start()
        {
            _health.Initialize();
            _aiController.Initialize(this);
            _aiController.WakeUp();
        }
    }
}

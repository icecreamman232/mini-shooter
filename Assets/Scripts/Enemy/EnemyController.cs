using Shinrai.AI;
using UnityEngine;

namespace Shinrai.Entity
{
    public class EnemyController : EntityController
    {
        [SerializeField] private EnemyMovement _movement;
        [SerializeField] private EnemyAIController _aiController;
        
        public EnemyMovement Movement => _movement;

        private void Start()
        {
            _aiController.Initialize(this);
            _aiController.WakeUp();
        }
    }
}

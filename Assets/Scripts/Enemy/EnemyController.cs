using Shinrai.AI;
using Shinrai.Weapon;
using UnityEngine;

namespace Shinrai.Entity
{
    public class EnemyController : EntityController
    {
        [SerializeField] private EnemyMovement _movement;
        [SerializeField] private EnemyHealth _health;
        [SerializeField] private EnemyWeapon _weapon;
        [SerializeField] private EnemyAIController _aiController;
        
        
        public EnemyMovement Movement => _movement;
        public EnemyHealth Health => _health;
        public EnemyWeapon Weapon => _weapon;
        

        private void Start()
        {
            if (_weapon != null)
            {
                _weapon.Initialize(this);
            }
            
            _health.Initialize();
            _aiController.Initialize(this);
            _aiController.WakeUp();
        }
    }
}

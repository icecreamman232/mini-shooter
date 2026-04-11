using Shinrai.Core;
using Shinrai.Entity;
using UnityEngine;

namespace Shinrai.AI
{
    public class ShootPlayerAIBehavior : AIBehavior
    {
        private Transform _playerTransform;

        public override void OnEnterState(EnemyController controller)
        {
            if (_playerTransform == null)
            {
                _playerTransform = ServiceLocator.GetService<InGameDataManager>().PlayerTransform;
            }
            base.OnEnterState(controller);
        }

        public override void OnUpdate(EnemyController controller)
        {
            var directionToPlayer = (_playerTransform.position - controller.transform.position).normalized;
            //Try to shoot at player position
            controller.Weapon.Shoot(directionToPlayer);
            base.OnUpdate(controller);
        }
    }
}

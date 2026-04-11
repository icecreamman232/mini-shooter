using Shinrai.Core;
using Shinrai.Entity;
using UnityEngine;

namespace Shinrai.AI
{
    [CreateAssetMenu(fileName = "Follow Player AI Behavior", menuName = "AI Behavior/Follow Player")]
    public class FollowPlayerAIBehavior : AIBehavior
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
            if (_playerTransform == null)
            {
                _playerTransform = ServiceLocator.GetService<InGameDataManager>().PlayerTransform;
            }
            
            var directionToPlayer = (_playerTransform.position - controller.transform.position).normalized;
            controller.Movement.SetMoveDirection(directionToPlayer);
            base.OnUpdate(controller);
        }
        
        public override void OnExitState(EnemyController controller)
        {
            controller.Movement.StopMoving();
            base.OnExitState(controller);
        }
    }
}

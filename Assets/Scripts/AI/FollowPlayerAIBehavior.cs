using Shinrai.Core;
using UnityEngine;

namespace Shinrai.AI
{
    [CreateAssetMenu(fileName = "Follow Player AI Behavior", menuName = "AI Behavior/Follow Player")]
    public class FollowPlayerAIBehavior : AIBehavior
    {
        private Transform _playerTransform;

        public override void OnEnterState()
        {
            if (_playerTransform == null)
            {
                _playerTransform = ServiceLocator.GetService<InGameDataManager>().PlayerTransform;
            }
            base.OnEnterState();
        }

        public override void OnUpdate()
        {
            if (_playerTransform == null)
            {
                _playerTransform = ServiceLocator.GetService<InGameDataManager>().PlayerTransform;
            }
            
            var directionToPlayer = (_playerTransform.position - _controller.transform.position).normalized;
            _controller.Movement.SetMoveDirection(directionToPlayer);
            base.OnUpdate();
        }
        
        public override void OnExitState()
        {
            _controller.Movement.StopMoving();
            base.OnExitState();
        }
    }
}

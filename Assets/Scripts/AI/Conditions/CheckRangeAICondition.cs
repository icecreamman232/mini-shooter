using Shinrai.Core;
using Shinrai.Entity;
using UnityEngine;

namespace Shinrai.AI
{
    /// <summary>
    /// Condition to check if player is in range of enemy
    /// </summary>
    public class CheckRangeAICondition : AICondition
    {
        [SerializeField] private float _rangeToCheck;
        private Transform _playerController;
        
        public override bool IsConditionMet(EnemyController controller)
        {
            if (_playerController == null)
            {
                _playerController = ServiceLocator.GetService<InGameDataManager>().PlayerTransform;
            }
            var distanceToPlayer = Vector2.Distance(controller.transform.position, _playerController.position);
            return distanceToPlayer <= _rangeToCheck;
        }
    }
}

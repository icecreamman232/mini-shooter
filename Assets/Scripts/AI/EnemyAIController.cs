using Shinrai.Entity;
using UnityEngine;

namespace Shinrai.AI
{
    public class EnemyAIController : MonoBehaviour
    {
        [SerializeField] private EnemyAIState[] _states;
        private EnemyAIState _currentState;
        private EnemyController _controller;
        
        public void Initialize(EnemyController controller)
        {
            _controller = controller;
            foreach (var state in _states)
            {
                state.Behavior.Initialize(controller);
            }
        }

        public void WakeUp()
        {
            _currentState = _states[0];
            _currentState.Behavior.OnEnterState(_controller);
        }

        private void FixedUpdate()
        {
            _currentState.Behavior.OnUpdate(_controller);
            if (_currentState.Condition != null &&
                _currentState.Condition.IsConditionMet(_controller))
            {
               if (_currentState.TrueState != null)
               {
                   _currentState = _currentState.TrueState;
                   _currentState.Behavior.OnEnterState(_controller);
               }
            }
        }
    }
}

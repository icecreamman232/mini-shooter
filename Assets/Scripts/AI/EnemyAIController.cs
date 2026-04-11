using System;
using Shinrai.Entity;
using UnityEngine;

namespace Shinrai.AI
{
    public class EnemyAIController : MonoBehaviour
    {
        [SerializeField] private EnemyAIState[] _states;
        private int _currentStateIndex;

        public void Initialize(EnemyController controller)
        {
            foreach (var state in _states)
            {
                state.Behavior.Initialize(controller);
            }
        }

        public void WakeUp()
        {
            _states[0].Behavior.OnEnterState();
            _currentStateIndex = 0;
        }

        private void FixedUpdate()
        {
            _states[_currentStateIndex].Behavior.OnUpdate();
        }
    }

    [Serializable]
    public class EnemyAIState
    {
        public AIBehavior Behavior;
    }
}

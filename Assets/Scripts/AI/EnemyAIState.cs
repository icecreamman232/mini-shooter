using System;
using UnityEngine;

namespace Shinrai.AI
{
    public class EnemyAIState : MonoBehaviour
    {
        public AIBehavior Behavior;
        public AICondition Condition;
        /// <summary>
        /// Next state to transition to if condition is met
        /// </summary>
        public EnemyAIState TrueState;
        /// <summary>
        /// Next state to transition to if condition is not met
        /// </summary>
        public EnemyAIState FalseState;
    }
}
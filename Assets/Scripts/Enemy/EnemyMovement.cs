
using System;
using UnityEngine;

namespace Shinrai.Entity
{
    public class EnemyMovement : EntityMovement
    {
        [SerializeField] private Transform _flipPivot;
        [SerializeField] private Animator _animator;
        
        private int _isMovingAnimBool = Animator.StringToHash("is_moving");

        private void FixedUpdate()
        {
            
        }
    }
}


using UnityEngine;

namespace Shinrai.Entity
{
    public class EnemyMovement : EntityMovement
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Animator _animator;
        private Vector2 _moveDirection;
        private int _isMovingAnimBool = Animator.StringToHash("is_running");

        private void FixedUpdate()
        {
            var deltaMovement = _moveDirection * (_speed * Time.fixedDeltaTime);
            _rigidbody.MovePosition( _rigidbody.position + deltaMovement);
        }

        public void SetMoveDirection(Vector2 direction)
        {
            _moveDirection = direction;
            _animator.SetBool(_isMovingAnimBool, _moveDirection.magnitude > 0.1f);
            _spriteRenderer.flipX = _moveDirection.x < 0;
        }

        public void StopMoving()
        {
            _moveDirection = Vector2.zero;
            _animator.SetBool(_isMovingAnimBool, false);
        }
    }
}

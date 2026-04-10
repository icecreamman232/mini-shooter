using Shinrai.Core;
using UnityEngine;

namespace Shinrai.Entity
{
    public class PlayerMovement : EntityMovement
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody2D _rigidbody;
        private Vector2 _moveDirection;
        private int _isRunningAnimBool = Animator.StringToHash("is_running");

        
        public void Initialize()
        {
            ServiceLocator.GetService<InputService>().MoveInputCallback += OnReceiveMoveInput;
        }

        public void CleanUp()
        {
            ServiceLocator.GetService<InputService>().MoveInputCallback -= OnReceiveMoveInput;
        }

        private void FixedUpdate()
        {
            var deltaMovement = _moveDirection * (_speed * Time.fixedDeltaTime);
            _rigidbody.MovePosition( _rigidbody.position + deltaMovement);
        }


        private void OnReceiveMoveInput(Vector2 input)
        {
            _moveDirection = input;
            _animator.SetBool(_isRunningAnimBool, input.magnitude > 0.1f);
        }
    }
}

using Shinrai.Core;
using Shinrai.Modifiers;
using UnityEngine;

namespace Shinrai.Entity
{
    public class PlayerMovement : EntityMovement
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody2D _rigidbody;
        private Vector2 _moveDirection;
        private int _isRunningAnimBool = Animator.StringToHash("is_running");
        
        private StatComponent _statComponent;
        
        public void Initialize(StatComponent statComponent)
        {
            ServiceLocator.GetService<InputService>().MoveInputCallback += OnReceiveMoveInput;
            _statComponent = statComponent;
            _speed = _statComponent.GetBase(StatTarget.MoveSpeed);
            _statComponent.OnStatChanged += OnStatChanged;
        }
        
        public void CleanUp()
        {
            ServiceLocator.GetService<InputService>().MoveInputCallback -= OnReceiveMoveInput;
            _statComponent.OnStatChanged -= OnStatChanged;
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
        
        private void OnStatChanged(StatComponent.StatChangeEvent obj)
        {
            
        }
    }
}

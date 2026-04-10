using System;
using Shinrai.Core;
using UnityEngine;

namespace Shinrai.Entity
{
    public class PlayerMovement : EntityMovement
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        private Vector2 _moveDirection;
        
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
        }
    }
}

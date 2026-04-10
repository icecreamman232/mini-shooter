using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shinrai.Core
{
    public class InputService : MonoBehaviour, IGameService, IBootStrap
    {
        private InputAction _moveAction;
        private bool _isActive;
        private Camera _camera;

        public Action<Vector2> MoveInputCallback;
        

        public void Install()
        {
            ServiceLocator.RegisterService(this);
            _camera = Camera.main;
            FindActions();
            RegisterActions();
            _isActive = true;
        }

        public void Uninstall()
        {
            _isActive = false;
        }

        private void Update()
        {
            if (!_isActive) return;
            
            MoveInputCallback?.Invoke(_moveAction.ReadValue<Vector2>());
        }

        private void FindActions()
        {
            _moveAction = InputSystem.actions.FindAction("Move");
        }

        private void RegisterActions()
        {
            
        }
    }
}
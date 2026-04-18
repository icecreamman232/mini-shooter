using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shinrai.Core
{
    public class InputService : MonoBehaviour, IGameService, IBootStrap
    {
        private InputAction _moveAction;
        private InputAction _shootAction;
        private InputAction _interactAction;
        private bool _isActive;
        private Camera _camera;

        public Action<Vector2> MoveInputCallback;
        public Action ShootInputCallback;
        public Action InteractInputCallback;
        public Action<Vector2> WorldMousePositionCallback;
        

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
            UnregisterActions();
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
        }

        private void Update()
        {
            if (!_isActive) return;
            
            MoveInputCallback?.Invoke(_moveAction.ReadValue<Vector2>());
            WorldMousePositionCallback?.Invoke(ComputeWorldMousePosition());
        }

        public Vector2 GetWorldMousePosition()
        {
            return ComputeWorldMousePosition();
        }

        private Vector2 ComputeWorldMousePosition()
        {
            var mousePosition = Mouse.current.position.ReadValue();
            mousePosition = _camera.ScreenToWorldPoint(mousePosition);
            return mousePosition;
        }

        private void FindActions()
        {
            _moveAction = InputSystem.actions.FindAction("Move");
            _shootAction = InputSystem.actions.FindAction("Attack");
            _interactAction = InputSystem.actions.FindAction("Interact");
        }

        private void RegisterActions()
        {
            _shootAction.performed += ShootActionOnPerformed;
            _interactAction.performed += InteractActionOnPerformed;
        }

        private void UnregisterActions()
        {
            _shootAction.performed -= ShootActionOnPerformed;
            _interactAction.performed -= InteractActionOnPerformed;
        }

        private void ShootActionOnPerformed(InputAction.CallbackContext callbackContext)
        {
            ShootInputCallback?.Invoke();
        }
        
        private void InteractActionOnPerformed(InputAction.CallbackContext callbackContext)
        {
            InteractInputCallback?.Invoke();
        }
    }
}
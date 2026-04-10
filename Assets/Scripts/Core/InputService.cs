using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shinrai.Core
{
    public class InputService : MonoBehaviour, IGameService, IBootStrap
    {
        private InputAction _moveAction;
        private InputAction _shootAction;
        private bool _isActive;
        private Camera _camera;

        public Action<Vector2> MoveInputCallback;
        public Action ShootInputCallback;
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
        }

        private void RegisterActions()
        {
            _shootAction.performed += ShootActionOnPerformed;
        }

        private void UnregisterActions()
        {
            _shootAction.performed -= ShootActionOnPerformed;
        }

        private void ShootActionOnPerformed(InputAction.CallbackContext callbackContext)
        {
            Debug.Log("Shoot action performed");
            ShootInputCallback?.Invoke();
        }
    }
}
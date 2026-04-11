using System;
using UnityEngine;

namespace Shinrai.Core
{
    public class CameraController : MonoBehaviour, IGameService
    {
        [SerializeField] private float _followSpeed;
        [SerializeField] private Camera _camera;
        private Transform _playerRef;
        private bool _isActive;

        private void Awake()
        {
            ServiceLocator.RegisterService(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.UnregisterService<CameraController>();
        }

        private void FixedUpdate()
        {
            if (!_isActive) return;
            Vector3 targetPosition = Vector3.Lerp(transform.position, _playerRef.position, _followSpeed * Time.fixedDeltaTime);
            _camera.transform.position = new Vector3(
                targetPosition.x,
                targetPosition.y,
                _camera.transform.position.z
            );
        }

        public void SetPlayer(Transform playerRef)
        {
            _playerRef = playerRef;
        }
        
        public void SetActive(bool isActive)
        {
            _isActive = isActive;
        }
    }
}

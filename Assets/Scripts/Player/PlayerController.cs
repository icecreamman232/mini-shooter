using UnityEngine;

namespace Shinrai.Entity
{
    public class PlayerController : EntityController
    {
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerHealth _playerHealth;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            _playerMovement.Initialize();
            _playerHealth.Initialize();
        }
    }
}

using Shinrai.Core;
using Shinrai.Weapon;
using UnityEngine;

namespace Shinrai.Entity
{
    public class PlayerController : EntityController
    {
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerHealth _playerHealth;
        [SerializeField] private PlayerWeapon _playerWeapon;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            _playerMovement.Initialize();
            _playerHealth.Initialize();
            _playerWeapon.Initialize(this);
            
            ServiceLocator.GetService<CameraController>().SetPlayer(transform);
            ServiceLocator.GetService<CameraController>().SetActive(true);
        }
    }
}

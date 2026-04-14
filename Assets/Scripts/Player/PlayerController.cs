using Shinrai.Core;
using Shinrai.Modifiers;
using Shinrai.Weapon;
using UnityEngine;

namespace Shinrai.Entity
{
    public class PlayerController : EntityController
    {
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerHealth _playerHealth;
        [SerializeField] private PlayerWeapon _playerWeapon;
        [SerializeField] private PlayerInventory _playerInventory;
        [SerializeField] private StatComponent _statComponent;
        public PlayerInventory PlayerInventory => _playerInventory;
        public PlayerHealth Health => _playerHealth;
        public PlayerMovement Movement => _playerMovement;
        public PlayerWeapon Weapon => _playerWeapon;
        
        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            _statComponent.Initialize(this);
            _playerInventory.Initialize();
            _playerMovement.Initialize(_statComponent);
            _playerHealth.Initialize(_statComponent);
            _playerWeapon.Initialize(this, _statComponent);
            
            
            ServiceLocator.GetService<CameraController>().SetPlayer(transform);
            ServiceLocator.GetService<CameraController>().SetActive(true);
            ServiceLocator.GetService<InGameDataManager>().AssignPlayer(transform);
        }
    }
}

using UnityEngine;

namespace Shinrai.Entity
{
    public class PlayerController : EntityController
    {
        [SerializeField] private PlayerMovement _playerMovement;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            _playerMovement.Initialize();
        }
    }
}

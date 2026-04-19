using Shinrai.Core;
using Shinrai.Entity;
using Shinrai.Modifiers;
using UnityEngine;


namespace Shinrai.Weapon
{
    public class PlayerWeapon : Weapon
    {
        [SerializeField] private Transform _shootPivot;
        private InputService _inputService;
        private StatComponent _statComponent;
        
        /// <summary>
        /// 2 bullets are fired parallel
        /// </summary>
        private bool _isDoubleShot;
        
        public void Initialize(EntityController owner, StatComponent statComponent)
        {
            _inputService = ServiceLocator.GetService<InputService>();
            _inputService.ShootInputCallback += OnShootInput;
            _statComponent = statComponent;
            _delayBetweenShots = Util.FireRateToDelay(_statComponent.GetFinal(StatTarget.FireRate));
            base.Initialize(owner);
        }

        private void OnDestroy()
        {
            _inputService.ShootInputCallback -= OnShootInput;
            _inputService = null;
        }

        private void OnShootInput()
        {
            var shootDirection = CalculateAimDirection(_inputService.GetWorldMousePosition(), _shootPivot.position);
            _delayBetweenShots = Util.FireRateToDelay(_statComponent.GetFinal(StatTarget.FireRate));
            Shoot(shootDirection, _shootPivot.position,
                _statComponent.GetFinal(StatTarget.MinDamage), 
                _statComponent.GetFinal(StatTarget.MaxDamage));
        }

        public override void Shoot(Vector2 shootDirection, Vector2 spawnPosition = default, float minDamage = 0, float maxDamage = 0)
        {
            _isDoubleShot = _statComponent.GetFinal(StatTarget.NumberOfShot) == 2;
            if (_isDoubleShot)
            {
                ShootDouble(shootDirection, spawnPosition, minDamage, maxDamage);
            }
            else
            {
                base.Shoot(shootDirection, spawnPosition, minDamage, maxDamage);
            }
        }

        private void ShootDouble(Vector2 shootDirection, Vector2 spawnPosition = default, float minDamage = 0, float maxDamage = 0)
        {
            if (!_canShoot) return;
            var leftProjectile = _projectilePool.GetPooledObject();
            leftProjectile.gameObject.SetActive(true);
            var rightProjectile = _projectilePool.GetPooledObject();
            rightProjectile.gameObject.SetActive(true);

            if (leftProjectile == null || rightProjectile == null) return;

            // Calculate perpendicular offset to the shoot direction
            float offsetDistance = 0.2f;
            Vector2 perpendicular = new Vector2(-shootDirection.y, shootDirection.x);
            
            var leftPosition = spawnPosition + perpendicular * offsetDistance;
            leftProjectile.transform.position = leftPosition;

            var rightPosition = spawnPosition - perpendicular * offsetDistance;
            rightProjectile.transform.position = rightPosition;
            
            leftProjectile.transform.up = shootDirection;
            rightProjectile.transform.up = shootDirection;
            
            leftProjectile.Spawn(_owner, minDamage, maxDamage);
            rightProjectile.Spawn(_owner, minDamage, maxDamage);
            
            StartCoroutine(OnDelayBetweenShots());
        }
    }
}

using Shinrai.Core;
using Shinrai.Entity;
using Shinrai.Modifiers;

namespace Shinrai.Weapon
{
    public class PlayerWeapon : Weapon
    {
        private InputService _inputService;
        private StatComponent _statComponent;
        
        public void Initialize(EntityController owner, StatComponent statComponent)
        {
            _inputService = ServiceLocator.GetService<InputService>();
            _inputService.ShootInputCallback += OnShootInput;
            _statComponent = statComponent;
            base.Initialize(owner);
        }

        private void OnDestroy()
        {
            _inputService.ShootInputCallback -= OnShootInput;
            _inputService = null;
        }

        private void OnShootInput()
        {
            var shootDirection = CalculateAimDirection(_inputService.GetWorldMousePosition());
            Shoot(shootDirection, 
                _statComponent.GetFinal(StatTarget.MinDamage), 
                _statComponent.GetFinal(StatTarget.MaxDamage));
        }
    }
}

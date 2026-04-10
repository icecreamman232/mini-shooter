using Shinrai.Core;

namespace Shinrai.Weapon
{
    public class PlayerWeapon : Weapon
    {
        private InputService _inputService;
        
        public override void Initialize()
        {
            _inputService = ServiceLocator.GetService<InputService>();
            _inputService.ShootInputCallback += OnShootInput;
            base.Initialize();
        }

        private void OnDestroy()
        {
            _inputService.ShootInputCallback -= OnShootInput;
            _inputService = null;
        }

        private void OnShootInput()
        {
            var shootDirection = CalculateAimDirection(_inputService.GetWorldMousePosition());
            Shoot(shootDirection);
        }
    }
}

using Shinrai.Core;
using UnityEngine;

namespace Shinrai.Entity
{
    public class PlayerModel : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Transform _weaponModelPivot;
        [SerializeField] private Transform _weaponModel;
        
        private InputService _inputService;

        public void Initialize()
        {
            _inputService = ServiceLocator.GetService<InputService>();
        }

        private void FixedUpdate()
        {
            var mouse_position = _inputService.GetWorldMousePosition();
            var directionToMouse = (mouse_position - (Vector2)transform.position).normalized;
            var angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
            var isFacingLeft = mouse_position.x < transform.position.x;
            FlipModel(mouse_position);
            RotateWeaponModel(angle, isFacingLeft);
        }

        private void FlipModel(Vector2 mousePosition)
        {
            _spriteRenderer.flipX = mousePosition.x < transform.position.x;
        }

        private void RotateWeaponModel(float angle,bool isFacingLeft)
        {
            _weaponModelPivot.rotation = Quaternion.Euler(0, 0, angle);

            var scale = _weaponModel.localScale;
            scale.y = isFacingLeft ? -1 : 1f;
            _weaponModel.localScale = scale;
        }
    }
}

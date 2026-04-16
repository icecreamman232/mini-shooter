using UnityEngine;

namespace Shinrai.Weapon
{
    public class DamageArea : MonoBehaviour
    {
        [SerializeField] private DamageComponent _damageComponent;

        private void OnTriggerEnter2D(Collider2D other)
        {
            _damageComponent.HandleCollision(other);
        }
    }
}

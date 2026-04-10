using UnityEngine;

namespace Shinrai.Weapon
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private ProjectilePool _projectilePool;

        
        public virtual void Initialize()
        {
            
        }
        
        public virtual void Shoot(Vector2 shootDirection)
        {
            var newProjectile = _projectilePool.GetPooledObject();
            if (newProjectile == null) return;
            
            newProjectile.transform.position = transform.position;
            newProjectile.transform.up = shootDirection;
            newProjectile.Spawn();
        }
        
        protected Vector2 CalculateAimDirection(Vector2 targetPosition)
        {
            return (targetPosition - (Vector2)transform.position).normalized;
        }
    }
}

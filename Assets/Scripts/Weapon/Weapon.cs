using System.Collections;
using Shinrai.Entity;
using UnityEngine;

namespace Shinrai.Weapon
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private ProjectilePool _projectilePool; 
        [SerializeField] private float _delayBetweenShots;

        protected bool _canShoot = true;
        protected EntityController _owner;
       
        public virtual void Initialize(EntityController owner)
        {
            _owner = owner;
        }
        
        public virtual void Shoot(Vector2 shootDirection)
        {
            if (!_canShoot) return;
            var newProjectile = _projectilePool.GetPooledObject();
            if (newProjectile == null) return;
            
            newProjectile.transform.position = transform.position;
            newProjectile.transform.up = shootDirection;
            newProjectile.Spawn(_owner);
            StartCoroutine(OnDelayBetweenShots());
        }
        
        protected Vector2 CalculateAimDirection(Vector2 targetPosition)
        {
            return (targetPosition - (Vector2)transform.position).normalized;
        }
        
        private IEnumerator OnDelayBetweenShots()
        {
            _canShoot = false;
            yield return new WaitForSeconds(_delayBetweenShots);
            _canShoot = true;
        }
    }
}

using System.Collections;
using Shinrai.Entity;
using UnityEngine;

namespace Shinrai.Weapon
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] protected ProjectilePool _projectilePool; 
        [SerializeField] protected float _delayBetweenShots;
        
        protected int _numberOfShot;
        protected bool _canShoot = true;
        protected EntityController _owner;
       
        public virtual void Initialize(EntityController owner)
        {
            _owner = owner;
        }
        
        public virtual void Shoot(Vector2 shootDirection, Vector2 spawnPosition = default, float minDamage = 0, float maxDamage = 0)
        {
            if (!_canShoot) return;
            var newProjectile = _projectilePool.GetPooledObject();
            if (newProjectile == null) return;
            if (spawnPosition != default)
            {
                newProjectile.transform.position = spawnPosition;
            }
            else
            {
                newProjectile.transform.position = transform.position;
            }
            
            newProjectile.transform.up = shootDirection;
            newProjectile.Spawn(_owner, minDamage, maxDamage);
            StartCoroutine(OnDelayBetweenShots());
        }
        
        protected Vector2 CalculateAimDirection(Vector2 targetPosition, Vector2 shootPosition)
        {
            return (targetPosition - shootPosition).normalized;
        }
        
        protected IEnumerator OnDelayBetweenShots()
        {
            _canShoot = false;
            yield return new WaitForSeconds(_delayBetweenShots);
            _canShoot = true;
        }
    }
}

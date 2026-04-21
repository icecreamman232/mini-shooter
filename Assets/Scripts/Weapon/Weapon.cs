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
        
        public virtual void Shoot(Vector2 shootDirection, Vector2 spawnPosition = default, float minDamage = 0, float maxDamage = 0, float projectileSpeed = 0)
        {
            if (!_canShoot) return;
            var newProjectile = _projectilePool.GetPooledObject();
            if (newProjectile == null) return;


            newProjectile.Configure()
                .WithOwner(_owner)
                .AtPosition(spawnPosition != default ? spawnPosition : transform.position)
                .WithDamage(minDamage, maxDamage)
                .WithDirection(shootDirection)
                .WithSpeed(projectileSpeed != 0 ? projectileSpeed : newProjectile.Speed)
                .Build();

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

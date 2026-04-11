using Shinrai.Entity;
using UnityEngine;

namespace Shinrai.Weapon
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] protected float _speed;
        [SerializeField] [Min(0)] protected float _range;
        [SerializeField] protected DamageComponent _damageComponent;
        
        protected float _travelDistance;
        protected Vector2 _startPosition;
        protected bool _isActive;
        
        
        private void Update()
        {
            if (!_isActive) return;
            transform.position += transform.up * (_speed * Time.deltaTime);
            _travelDistance = Vector2.Distance(transform.position, _startPosition);
            if (_travelDistance >= _range)
            {
                DestroyProjectile();
            }
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_damageComponent.HandleCollision(other))
            {
                DestroyProjectile();
            }
        }

        public void Spawn(EntityController owner)
        {
            _isActive = true;
            _damageComponent.AssignOwner(owner);
            gameObject.SetActive(true);
            _travelDistance = 0;
            _startPosition = transform.position;
        }
        
        
        /// <summary>
        /// Override this method to destroy the projectile. This will call to projectile pool to disable the projectile.
        /// </summary>
        protected virtual void DestroyProjectile()
        {
            _isActive = false;
            this.gameObject.SetActive(false);
        }
    }
}
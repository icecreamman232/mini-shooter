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

        public float Speed => _speed;
        public float Range => _range;
        
        
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

        public void Spawn()
        {
            _isActive = true;
            gameObject.SetActive(true);
            _travelDistance = 0;
            _startPosition = transform.position;
        }

        public ProjectileBuilder Configure()
        {
            return new ProjectileBuilder(this);
        }

        // Builder helper methods (internal, called by ProjectileBuilder)
        internal void AssignOwner(EntityController owner)
        {
            _damageComponent.AssignOwner(owner);
        }

        internal void SetDamage(float minDamage, float maxDamage)
        {
            _damageComponent.SetDamage(minDamage, maxDamage);
        }

        internal void SetSpeed(float speed)
        {
            _speed = speed;
        }

        internal void SetRange(float range)
        {
            _range = range;
        }

        internal void Activate()
        {
            _isActive = true;
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
using Shinrai.Core;
using Shinrai.Entity;
using UnityEngine;

namespace Shinrai.Weapon
{
    public class DamageComponent : MonoBehaviour
    {
        [SerializeField] private LayerMask _damageableLayers;
        [SerializeField] private float _minDamage;
        [SerializeField] private float _maxDamage;
        private EntityController _owner;
        

        public virtual bool HandleCollision(Collider2D target)
        {
            if (!Util.IsInLayerMask(target.gameObject.layer, _damageableLayers)) return false;

            //Obstacle wont have a health component attached to it so we return true here
            if (target.gameObject.layer == LayerMask.NameToLayer("Obstacle")) return true;
            
            var health = target.GetComponent<EntityHealth>();
            if (health == null) return false;
            DealDamage(health);
            return true;
        }

        public virtual void DealDamage(EntityHealth target)
        {
            target.TakeDamage(GetDamage(), _owner);
        }
        
        protected virtual float GetDamage()
        {
            return Random.Range(_minDamage, _maxDamage);
        }

        public void AssignOwner(EntityController owner)
        {
            _owner = owner;
        }
    }
}

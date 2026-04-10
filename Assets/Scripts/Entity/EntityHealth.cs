using System.Collections;
using UnityEngine;

namespace Shinrai.Entity
{
    public class EntityHealth : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] protected float _currentHealth;
        [SerializeField] protected float _maxHealth;
        [SerializeField] protected float _invulnerableTime;
        
        protected bool _isInvulnerable;
        public bool IsDead => _currentHealth <= 0;

        public virtual void Initialize()
        {
            _currentHealth = _maxHealth;
            _isInvulnerable = false;
        }

        public virtual void TakeDamage(float damage, EntityController source)
        {
            if (_isInvulnerable) return;
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                Kill();
            }
            else
            {
                StartCoroutine(OnInvulnerable());
            }
            UpdateHealthBar();
        }

        protected virtual void UpdateHealthBar()
        {
            
        }
        
        protected virtual void Kill()
        {
            _isInvulnerable = false;
        }

        protected virtual IEnumerator OnInvulnerable()
        {
            _isInvulnerable = true;
            yield return new WaitForSeconds(_invulnerableTime);
            _isInvulnerable = false;
        }
    }
}

using UnityEngine;

namespace Shinrai.Data
{
    [CreateAssetMenu(fileName = "Character Data", menuName = "Characters Data")]
    public class CharacterData : ScriptableObject
    {
        [SerializeField] private float _defaultHealth;
        [SerializeField] private float _defaultSpeed;
        [SerializeField] private float _defaultMinDamage;
        [SerializeField] private float _defaultMaxDamage;
        [SerializeField] private float _defaultFireRate;
        [SerializeField] private float _projectileSpeed;
        
        public float DefaultHealth => _defaultHealth;
        public float DefaultSpeed => _defaultSpeed;
        public float DefaultMinDamage => _defaultMinDamage;
        public float DefaultMaxDamage => _defaultMaxDamage;
        public float DefaultFireRate => _defaultFireRate;
        public float ProjectileSpeed => _projectileSpeed;
    }
}

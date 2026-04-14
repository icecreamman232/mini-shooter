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
        
        public float DefaultHealth => _defaultHealth;
        public float DefaultSpeed => _defaultSpeed;
        public float DefaultMinDamage => _defaultMinDamage;
        public float DefaultMaxDamage => _defaultMaxDamage;
    }
}

using UnityEngine;

namespace Shinrai.Entity
{
    public class EntityMovement : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] protected float _speed;
        
        public float Speed => _speed;
    }
}

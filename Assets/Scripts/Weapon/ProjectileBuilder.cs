using Shinrai.Entity;
using UnityEngine;

namespace Shinrai.Weapon
{
    public class ProjectileBuilder
    {
        private readonly Projectile _projectile;
        private EntityController _owner;
        private float _minDamage;
        private float _maxDamage;
        private float _speed;
        private float _range;
        private Vector2 _position;
        private Vector2 _direction;

        public ProjectileBuilder(Projectile projectile)
        {
            _projectile = projectile;
            _speed = projectile.Speed;
            _range = projectile.Range;
        }

        public ProjectileBuilder WithOwner(EntityController owner)
        {
            _owner = owner;
            return this;
        }

        public ProjectileBuilder WithDamage(float minDamage, float maxDamage)
        {
            _minDamage = minDamage;
            _maxDamage = maxDamage;
            return this;
        }

        public ProjectileBuilder WithSpeed(float speed)
        {
            _speed = speed;
            return this;
        }

        public ProjectileBuilder WithRange(float range)
        {
            _range = range;
            return this;
        }

        public ProjectileBuilder AtPosition(Vector2 position)
        {
            _position = position;
            return this;
        }

        public ProjectileBuilder WithDirection(Vector2 direction)
        {
            _direction = direction;
            return this;
        }

        public Projectile Build()
        {
            if (_owner != null)
            {
                _projectile.AssignOwner(_owner);
            }

            if (_minDamage != 0 && _maxDamage != 0)
            {
                _projectile.SetDamage(_minDamage, _maxDamage);
            }

            _projectile.SetSpeed(_speed);
            _projectile.SetRange(_range);

            if (_position != default)
            {
                _projectile.transform.position = _position;
            }

            if (_direction != default)
            {
                _projectile.transform.up = _direction;
            }

            _projectile.Activate();

            return _projectile;
        }
    }
}

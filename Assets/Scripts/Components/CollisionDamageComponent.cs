using System;
using System.Collections;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class CollisionDamageComponent : MonoBehaviour
    {
        [SerializeField]
        private float _minimumMagnitude;

        [SerializeField]
        private float _damageModifier;

        [SerializeField]
        private float _angleThreshold;

        [SerializeField]
        private float _cooldown;

        private Rigidbody2D _rigidBody;
        private HealthComponent _healthComponent;
        private bool _canTakeDamage = true;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _healthComponent = GetComponent<HealthComponent>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!_canTakeDamage)
            {
                return;
            }

            var magnitude = collision.relativeVelocity.magnitude;
            if (magnitude < _minimumMagnitude)
            {
                return;
            }

            var angleBetween = AngleUtilities.AngleBetween(
                new Vector3(_rigidBody.velocity.x, _rigidBody.velocity.y, 0),
                new Vector3(collision.relativeVelocity.x, collision.relativeVelocity.y, 0));

            if (angleBetween > _angleThreshold)
            {
                return;
            }
            
            _healthComponent.UpdateHealth(Convert.ToInt32(-magnitude * _damageModifier));
            StartCoroutine(CooldownTimer());
        }

        private IEnumerator CooldownTimer()
        {
            _canTakeDamage = false;
            yield return new WaitForSeconds(_cooldown);
            _canTakeDamage = true;
        }
    }
}

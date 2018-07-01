using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    public class HealthComponent : MonoBehaviour
    {
        public event Action<HealthComponent> DeathEvent;

        public Slider HealthSlider;
        public GameObject Explosion;

        [SerializeField]
        private int _maxHealth;

        [SerializeField]
        private int _health;

        private void Start()
        {
            HealthSlider.maxValue = _maxHealth;
            HealthSlider.value = _health;
        }

        public void UpdateHealth(int change)
        {
            _health += change;

            if (_health > _maxHealth)
            {
                _health = _maxHealth;
            }
            else if (_health < 0)
            {
                _health = 0;
            }

            HealthSlider.value = _health;

            if (_health <= 0)
            {
                HandleDeath();
            }
        }

        private void HandleDeath()
        {
            var explosion = Instantiate(Explosion, transform.position, Quaternion.identity);
            Destroy(explosion, 1);

            if (DeathEvent != null)
            {
                DeathEvent(this);
            }
        }
    }
}

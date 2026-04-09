using System;
using Interfaces;
using UnityEngine;

namespace Base
{
    public class BaseCharacter : MonoBehaviour, IDamageable
    {
        [field: SerializeField] public float TotalHealth { get; private set; }

        private float _currentHealth;

        private void Awake()
        {
            _currentHealth = TotalHealth;
        }

        public virtual void TakeDamage(float damage)
        {
            _currentHealth -= damage;

            if (_currentHealth <= 0)
            {
                Destroy(gameObject);
            } 
        }

        public void ShowHealth()
        {
            Debug.Log("El enemigo tiene " + _currentHealth);
        }
    }
}
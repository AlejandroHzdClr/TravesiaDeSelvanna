using System;
using Interfaces;
using UnityEngine;

namespace Base
{
    public class BaseCharacter : MonoBehaviour, IDamageable
    {
        [field: SerializeField] public float TotalHealth { get; private set; }

        protected float CurrentHealth;

        public virtual void Awake()
        {
            CurrentHealth = TotalHealth;
        }

        public virtual void TakeDamage(float damage)
        {
            CurrentHealth -= damage;

            if (CurrentHealth <= 0)
            {
                Destroy(gameObject);
            } 
        }

        public void ShowHealth()
        {
            Debug.Log("El enemigo tiene " + CurrentHealth);
        }
    }
}
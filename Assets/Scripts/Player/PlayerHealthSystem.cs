using System;
using Base;
using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerHealthSystem : BaseCharacter
    {
        
        public override void Awake()
        {
            
        }

        private void Start()
        {
            CurrentHealth = GameManager.Instance.playerHealth;
        }


        public override void TakeDamage(float damage)
        {
            CurrentHealth -= damage;
            
            EventManager.HealthChanged(CurrentHealth);

            if (CurrentHealth <= 0)
            {
                Destroy(gameObject);
            } 
        }
    }
}
using System;
using Enemy;
using Interfaces;
using UnityEngine;

namespace Base
{
    public class BaseCharacter : MonoBehaviour, IDamageable
    {
        [field: SerializeField] public float TotalHealth { get; private set; }
        [SerializeField] private bool isEnemy;
        [SerializeField] private AudioClip audioClip;
        
        private AudioSource audioSource;
        private EnemyDying enemyDying;

        protected float CurrentHealth;

        public virtual void Awake()
        {
            CurrentHealth = TotalHealth;
            if (isEnemy)
            {
                audioSource = GetComponent<AudioSource>();
                enemyDying = GetComponent<EnemyDying>();
            }
        }

        public virtual void TakeDamage(float damage)
        {
            CurrentHealth -= damage;

            if (audioClip != null)
                AudioSource.PlayClipAtPoint(audioClip, transform.position);

            if (CurrentHealth <= 0)
            {
                if (isEnemy)
                {
                    enemyDying.Die();
                }
                else
                {
                    Destroy(gameObject);
                }
            } 
        }

        public void ShowHealth()
        {
            Debug.Log("El enemigo tiene " + CurrentHealth);
        }
    }
}
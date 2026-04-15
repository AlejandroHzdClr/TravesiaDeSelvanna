using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Health
{
    public class LifeBorderController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> borders;

        
        private float healthRemaining;
        private float currentHealth;
        private float maxHealth= 100f;

        private void Start()
        {
            currentHealth = GameManager.Instance.playerHealth;
            healthRemaining = maxHealth - currentHealth;
        }

        private void OnEnable()
        {
            EventManager.PlayerHealthChanged += UpdateHealthBorders;
        }
        
        private void OnDisable()
        {
            EventManager.PlayerHealthChanged -= UpdateHealthBorders;
        }

        private void UpdateHealthBorders(float obj)
        {
            healthRemaining = maxHealth - obj;

            int bloques = Mathf.FloorToInt(healthRemaining / 20f);

            for (int i = 0; i < borders.Count; i++)
            {
                borders[i].SetActive(i<bloques);
            }
        }
    }
}

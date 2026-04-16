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
            UpdateHealthBorders(currentHealth);
        }

        private void UpdateHealthBorders(float newHealth)
        {
            float lostHealth = maxHealth - newHealth;
            int bloques = Mathf.FloorToInt(lostHealth / 20f);

            for (int i = 0; i < borders.Count; i++)
            {
                borders[i].SetActive(i < bloques);
            }
        }


        private void OnEnable()
        {
            EventManager.Instance.PlayerHealthChanged += UpdateHealthBorders;
        }
        
        private void OnDisable()
        {
            EventManager.Instance.PlayerHealthChanged -= UpdateHealthBorders;
        }

    }
}

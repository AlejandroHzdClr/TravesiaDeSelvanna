using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Health
{
    public class LifeBorderController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> borders;

        private float maxHealth = 100f;
        private bool isSubscribed = false;

        private void Start()
        {
            float currentHealth = GameManager.Instance.playerHealth;
            UpdateHealthBorders(currentHealth);
        }

        private void OnEnable()
        {
            StartCoroutine(WaitForEventManager());
        }

        private IEnumerator WaitForEventManager()
        {
            while (EventManager.Instance == null)
                yield return null;

            if (!isSubscribed)
            {
                EventManager.Instance.PlayerHealthChanged += UpdateHealthBorders;
                isSubscribed = true;
            }
        }

        private void OnDisable()
        {
            if (isSubscribed && EventManager.Instance != null)
            {
                EventManager.Instance.PlayerHealthChanged -= UpdateHealthBorders;
                isSubscribed = false;
            }
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
    }
}
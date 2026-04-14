using System;
using Interfaces;
using Managers;
using UnityEngine;

namespace Collectables
{
    public class Pills : MonoBehaviour, ICollectable<float>
    {
        [SerializeField] private float heathHealed;
        [SerializeField] private float timeOfLife;

        private float currentTime = 0f;
        private void Update()
        {
            currentTime += Time.deltaTime;
            if (currentTime >= timeOfLife)
            {
                Destroy(this.gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                BeingCollected(heathHealed);
                Destroy(this.gameObject);
            }
        }

        public void BeingCollected(float heal)
        {
            GameManager.Instance.playerHealth += heal;
            if (GameManager.Instance.playerHealth >= 100)
            {
                GameManager.Instance.playerHealth = 100f;
            }
        }
    }
    
}

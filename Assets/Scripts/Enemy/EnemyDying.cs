using System;
using Managers;
using UnityEngine;

namespace Enemy
{
    public class EnemyDying : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private bool isChild;
        
        [Range(0f,1f)]
        [SerializeField] private float prob;

        public void Die()
        {
            Debug.Log("ha sido destruido");
            if (GameManager.Instance.rng.NextDouble() < prob)
            {
                Debug.Log("Genero pill");
                Instantiate(prefab, transform.position, Quaternion.identity);
            }

            if (!isChild)
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(transform.root.gameObject);
            }
            
        }
    }
}
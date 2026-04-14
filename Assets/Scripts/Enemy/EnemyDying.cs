using System;
using Managers;
using UnityEngine;

namespace Enemy
{
    public class EnemyDying : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        
        [Range(0f,1f)]
        [SerializeField] private float prob;

        private void OnDestroy()
        {
            if (GameManager.Instance.rng.NextDouble() < prob)
            {
                Instantiate(prefab, transform.position, Quaternion.identity);
            }
        }
    }
}
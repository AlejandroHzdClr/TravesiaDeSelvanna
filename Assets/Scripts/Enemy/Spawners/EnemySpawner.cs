using System.Collections;
using UnityEngine;

namespace Enemy.Spawners
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject enemy;
        [SerializeField] private float intervalo;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            StartCoroutine(SpawnNewEnemy());
        }

        private IEnumerator SpawnNewEnemy()
        {
            while (true)
            {
                Instantiate(enemy, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(intervalo);
            }
        }
    }
}

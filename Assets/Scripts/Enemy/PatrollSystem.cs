using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class PatrollSystem : MonoBehaviour
{

    [SerializeField] private Transform patrolPoints;
    [SerializeField] private float monsterSpeed;
    [SerializeField] private float damage;
    
    private List<Vector3> patrolPositions = new List<Vector3>();
    private Vector3 currentDestination;
    private int currentIndex = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        foreach (Transform patrolPoint in patrolPoints)
        {
            patrolPositions.Add(patrolPoint.position);
        }

        StartCoroutine(PatrolAndWait());
    }

    private IEnumerator PatrolAndWait()
    {
        while (true)
        {
            CalculateNewDestination();
            FaceToDestination();
            while (transform.position != currentDestination)
            {
                transform.position = Vector3.MoveTowards(transform.position, 
                    currentDestination, monsterSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.75f));
            currentIndex = (currentIndex + 1) % patrolPositions.Count;
        }
        
    }
    
    private void CalculateNewDestination()
    {
        currentDestination = patrolPositions[currentIndex];
    }

    private void FaceToDestination()
    {
        float x = currentDestination.x - transform.position.x;
        if (Mathf.Approximately(Mathf.Sign(x), 1f))
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (Mathf.Approximately(Mathf.Sign(x), -1f))
        {
            transform.eulerAngles = Vector3.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
            damageable.ShowHealth();
        }
    }
}

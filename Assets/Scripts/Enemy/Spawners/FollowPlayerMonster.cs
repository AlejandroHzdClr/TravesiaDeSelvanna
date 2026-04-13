using System;
using System.Numerics;
using TMPro;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Enemy.Spawners
{
    public class FollowPlayerMonster : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float damage;

        private Transform target;
        private bool isFollowing;

        // Update is called once per frame
        void Update()
        {
            if (isFollowing && target != null)
            {
                Vector3 follow = (target.position - transform.position).normalized;
                transform.position += follow * (speed * Time.deltaTime);
            }
            else
            {
                Vector3 normal = Vector3.right;
                transform.position += normal * (speed * Time.deltaTime);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                target = other.transform;
                isFollowing = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                isFollowing = false;
            }
        }
    }
}

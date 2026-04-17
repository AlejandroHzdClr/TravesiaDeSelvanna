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
        [SerializeField] private bool isMoving;
        [SerializeField] private AudioClip audioClip;

        private AudioSource audioSource;
        private Transform target;
        private Transform root;
        private bool isFollowing;

        private void Awake()
        {
            root = transform.parent;
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            if (isFollowing && target != null)
            {
                Vector3 follow = (target.position - root.position).normalized;
                root.position += follow * (speed * Time.deltaTime);
            }
            else
            {
                if (isMoving)
                {
                    Vector3 normal = root.right;
                    root.position += normal * (speed * Time.deltaTime);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                target = other.transform;
                isFollowing = true;
                audioSource.PlayOneShot(audioClip);
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

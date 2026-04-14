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

        private Transform target;
        private Transform root;
        private bool isFollowing;

        private void Awake()
        {
            root = transform.parent;
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
                Vector3 normal = Vector3.right;
                root.position += normal * (speed * Time.deltaTime);
            }
        }
    }
}

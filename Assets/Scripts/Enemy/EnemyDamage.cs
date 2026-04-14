using Interfaces;
using UnityEngine;

namespace Enemy
{
    public class EnemyDamage : MonoBehaviour
    {
        [SerializeField] private float damage;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
            }
        }

    }
}
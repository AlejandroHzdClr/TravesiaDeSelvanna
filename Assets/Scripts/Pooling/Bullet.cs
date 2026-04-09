using Interfaces;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage;
    public ObjectPool<Bullet> MyPool { get; set; }
    
    private void Update()
    {
        transform.Translate(Vector3.right * (5f * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            MyPool.Release(this);
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
                damageable.ShowHealth();
            }
            MyPool.Release(this);
        }
    }
}

using UnityEngine;
using UnityEngine.Pool;

public class ShootSystem : MonoBehaviour
{

    [SerializeField]private Bullet ball;
    public ObjectPool<Bullet> pool { get; set; }
    private Vector3 mousePoint;

    private bool isCharging = false;
    private float charge = 0f;

    [SerializeField] private float maxCharge = 2f;
    [SerializeField] private float chargeSpeed = 1f;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        pool = new ObjectPool<Bullet>(CreateBullet, GetBullet, ReleaseBullet);
    }

    private Bullet CreateBullet()
    {
        Bullet copy = Instantiate(ball, transform.position, transform.rotation);
        copy.MyPool = pool;
        return copy;
    }

    private void GetBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
        bullet.transform.position = transform.position;
        Vector3 direction = (mousePoint - transform.position).normalized;
        bullet.transform.right = direction;
    }

    private void ReleaseBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isCharging = true;
            charge = 0f;
        }

        if (Input.GetMouseButton(0) && isCharging)
        {
            charge += chargeSpeed * Time.deltaTime;
            charge = Mathf.Clamp(charge, 0f, maxCharge);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (charge >= maxCharge)
            {
                mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePoint.z = 0f;
                pool.Get();
            }

            isCharging = false;
        }
    }

}

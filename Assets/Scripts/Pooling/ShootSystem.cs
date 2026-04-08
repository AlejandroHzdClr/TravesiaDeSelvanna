using UnityEngine;
using UnityEngine.Pool;

public class ShootSystem : MonoBehaviour
{
    [SerializeField] private Bullet ball;
    public ObjectPool<Bullet> pool { get; set; }

    private Vector3 mousePoint;

    private bool isCharging = false;
    private float charge = 0f;

    [SerializeField] private float maxCharge = 2f;
    [SerializeField] private float chargeSpeed = 1f;

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

    private Vector3 GetMouseWorldPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.forward, Vector3.zero);

        if (plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }

        return transform.position;
    }

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
                mousePoint = GetMouseWorldPoint();
                pool.Get();
            }

            isCharging = false;
        }
    }
}
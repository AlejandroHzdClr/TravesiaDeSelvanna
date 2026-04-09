using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.InputSystem;

public class ShootSystem : MonoBehaviour
{
    [SerializeField] private Bullet ball;
    public ObjectPool<Bullet> pool { get; set; }

    private PlayerInput controls;

    private Vector3 mousePoint;

    private bool isCharging = false;
    private float charge = 0f;

    [SerializeField] private float maxCharge = 2f;
    [SerializeField] private float chargeSpeed = 1f;

    void Awake()
    {
        controls = new PlayerInput();

        // START: cuando se presiona el botón
        controls.Gameplay.Shoot.started += _ =>
        {
            isCharging = true;
            charge = 0f;
        };

        // CANCELED: cuando se suelta el botón
        controls.Gameplay.Shoot.canceled += _ =>
        {
            if (charge >= maxCharge)
            {
                mousePoint = GetMouseWorldPoint();
                pool.Get();
            }

            isCharging = false;
        };

        pool = new ObjectPool<Bullet>(CreateBullet, GetBullet, ReleaseBullet);
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

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
        Vector2 mouseScreen = controls.Gameplay.Aim.ReadValue<Vector2>();

        Ray ray = Camera.main.ScreenPointToRay(mouseScreen);
        Plane plane = new Plane(Vector3.forward, Vector3.zero);

        if (plane.Raycast(ray, out float distance))
            return ray.GetPoint(distance);

        return transform.position;
    }

    void Update()
    {
        if (isCharging)
        {
            charge += chargeSpeed * Time.deltaTime;
            charge = Mathf.Clamp(charge, 0f, maxCharge);
        }
    }
}
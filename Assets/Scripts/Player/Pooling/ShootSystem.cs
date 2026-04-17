using ScriptableObjects;
using UnityEngine;
using UnityEngine.Pool;

public class ShootSystem : MonoBehaviour
{
    [SerializeField] private InputSystemSO inputSO;
    [SerializeField] private Bullet ball;

    public ObjectPool<Bullet> pool { get; private set; }

    private AudioSource audioSource;

    private bool isCharging = false;
    private float charge = 0f;

    [SerializeField] private float maxCharge = 2f;
    [SerializeField] private float chargeSpeed = 1f;

    [SerializeField] private AudioClip chargeSound;
    [SerializeField] private AudioClip releaseSound;

    private bool ignoreFirstFrame = true;
    private Vector2 aimScreenPos;
    private Vector3 mousePoint;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        pool = new ObjectPool<Bullet>(CreateBullet, GetBullet, ReleaseBullet);
    }

    private void OnEnable()
    {
        inputSO.OnShootStarted += StartCharge;
        inputSO.OnShootEnded += ReleaseShot;
        inputSO.OnAim += UpdateAim;
    }

    private void OnDisable()
    {
        inputSO.OnShootStarted -= StartCharge;
        inputSO.OnShootEnded -= ReleaseShot;
        inputSO.OnAim -= UpdateAim;
    }

    private void StartCharge()
    {
        if (ignoreFirstFrame) return;
        if (Time.timeScale == 0) return;

        isCharging = true;
        charge = 0f;

        if (chargeSound != null)
            audioSource.PlayOneShot(chargeSound);
    }

    private void ReleaseShot()
    {
        if (ignoreFirstFrame) return;
        if (Time.timeScale == 0) return;

        audioSource.Stop();

        if (charge >= maxCharge)
        {
            mousePoint = GetMouseWorldPoint();
            pool.Get();

            if (releaseSound != null)
                audioSource.PlayOneShot(releaseSound);
        }

        isCharging = false;
    }

    private void UpdateAim(Vector2 screenPos)
    {
        aimScreenPos = screenPos;
    }

    private Vector3 GetMouseWorldPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimScreenPos);
        Plane plane = new Plane(Vector3.forward, Vector3.zero);

        if (plane.Raycast(ray, out float distance))
            return ray.GetPoint(distance);

        return transform.position;
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

    private void Update()
    {
        if (ignoreFirstFrame)
        {
            ignoreFirstFrame = false;
            return;
        }

        if (isCharging)
        {
            charge += chargeSpeed * Time.deltaTime;
            charge = Mathf.Clamp(charge, 0f, maxCharge);
        }
    }
}

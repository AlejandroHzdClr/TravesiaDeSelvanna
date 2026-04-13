using UnityEngine;

namespace Player
{
    public class AimGaunlet : PlayerSystem
    {
        private PlayerInput controls;
        private PlayerMain playerMain;

        protected override void Awake()
        {
            base.Awake();
            controls = new PlayerInput();
        }

        private void Start()
        {
            playerMain = GetComponentInParent<PlayerMain>();

            if (playerMain.SlowPlaying)
            {
                controls.Terror.Enable();
                controls.Bosque.Disable();
            }
            else
            {
                controls.Bosque.Enable();
                controls.Terror.Disable();
            }
        }

        private Vector3 GetMouseWorldPoint()
        {
            Vector2 mouseScreen;
            if (playerMain.SlowPlaying)
            {
                mouseScreen = controls.Terror.Aim.ReadValue<Vector2>();
            }
            else
            {
                mouseScreen = controls.Bosque.Aim.ReadValue<Vector2>();
            }

            Ray ray = Camera.main.ScreenPointToRay(mouseScreen);
            Plane plane = new Plane(Vector3.forward, Vector3.zero);

            if (plane.Raycast(ray, out float distance))
                return ray.GetPoint(distance);

            return transform.position;
        }

        void Update()
        {
            Vector3 mouseWorld = GetMouseWorldPoint();
            Vector3 direction = mouseWorld - transform.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle += 90f;

            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
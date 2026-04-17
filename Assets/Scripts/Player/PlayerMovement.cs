using Managers;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : PlayerSystem
    {
        private static readonly int Jumping = Animator.StringToHash("Jumping");
        private static readonly int Walking = Animator.StringToHash("Walking");

        [SerializeField] private float radius;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float airControlMultiplier = 1.2f;
        [SerializeField] private InputSystemSO inputSO;
        [SerializeField] private AudioClip audioClip;

        private AudioSource audioSource;
        private float hInput;
        private float vInput;
        private int groundMask;
        private bool isGrounded;
        private bool jumpPressed;
        private bool canUseStairs;

        private PlayerInput input; // SOLO para WS

        protected override void Awake()
        {
            base.Awake();
            audioSource = GetComponent<AudioSource>();
            groundMask = LayerMask.GetMask("Ground");
            input = new PlayerInput();
        }

        private void OnEnable()
        {
            input.Enable();

            inputSO.OnMove += HandleMove;
            inputSO.OnJumpPressed += HandleJump;
            inputSO.OnInteractPressed += HandleInteract;
        }

        private void OnDisable()
        {
            inputSO.OnMove -= HandleMove;
            inputSO.OnJumpPressed -= HandleJump;
            inputSO.OnInteractPressed -= HandleInteract;

            input.Disable();
        }

        private void HandleMove(Vector2 move)
        {
            hInput = move.x;
        }

        private void HandleJump()
        {
            if (audioSource != null)
            {
                audioSource.PlayOneShot(audioClip);
            }
            jumpPressed = true;
        }

        private void HandleInteract()
        {
            EventManager.Instance.OnRelease();
        }

        private void Update()
        {
            if (canUseStairs && Main.SlowPlaying)
                vInput = input.Terror.WS.ReadValue<float>();
            else
                vInput = 0f;

            if (hInput > 0)
                transform.eulerAngles = Vector3.zero;
            else if (hInput < 0)
                transform.eulerAngles = new Vector3(0, 180, 0);

            Main.Anim.SetBool(Walking, hInput != 0);

            isGrounded = Physics2D.OverlapCircle(
                groundCheck.position,
                radius,
                groundMask
            );

            if (isGrounded && jumpPressed && !Main.SlowPlaying)
            {
                Main.Rb.AddForce(Vector2.up * Main.JumpForce, ForceMode2D.Impulse);
                jumpPressed = false;
            }

            Main.Anim.SetBool(Jumping, !isGrounded);
        }

        private void FixedUpdate()
        {
            float speed = Main.MovementSpeed;

            if (!isGrounded)
                speed *= airControlMultiplier;

            Vector2 vel = Main.Rb.linearVelocity;

            vel.x = hInput * speed;

            if (canUseStairs)
                vel.y = vInput * speed;

            Main.Rb.linearVelocity = vel;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Stairs"))
                canUseStairs = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Stairs"))
                canUseStairs = false;
        }

        private void OnDrawGizmosSelected()
        {
            if (groundCheck != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(groundCheck.position, radius);
            }
        }
    }
}

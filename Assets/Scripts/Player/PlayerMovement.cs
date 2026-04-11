using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : PlayerSystem
    {
        private static readonly int Jumping = Animator.StringToHash("Jumping");
        private static readonly int Walking = Animator.StringToHash("Walking");
    
        [SerializeField] private float radius;

        [Header("Ground Check")]
        [SerializeField] private Transform groundCheck;
        
        private float hInput;
        private int groundMask;
        private bool isGrounded;
        private bool jumpPressed;

        private PlayerInput input;
        [SerializeField] private float airControlMultiplier = 1.2f;

        protected override void Awake()
        {
            base.Awake();
            
            input = new PlayerInput();
            if (Main.SlowPlaying)
            {
                input.Terror.Enable();
                input.Bosque.Disable();
            }
            else
            {
                input.Bosque.Enable();
                input.Terror.Disable();
                input.Bosque.Jump.performed += OnJumpPerformed;
            }
            
            
            groundMask = LayerMask.GetMask("Ground");
        }

        private void OnJumpPerformed(InputAction.CallbackContext obj)
        {
            jumpPressed = true;
        }

        void Update()
        {
            if (Main.SlowPlaying)
            {
                hInput = input.Terror.AD.ReadValue<float>();
            }
            else
            {
                hInput = input.Bosque.AD.ReadValue<float>();
            }
            
            
            if (hInput>0)
            {
                transform.eulerAngles = Vector3.zero;
            }
            else if(hInput < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        
            Main.Anim.SetBool(Walking,hInput!=0);
        
            isGrounded = Physics2D.OverlapCircle(
                groundCheck.position,
                radius,
                groundMask
            );

            
            if (isGrounded && jumpPressed)
            {
                Main.Rb.AddForce(Vector2.up * Main.JumpForce, ForceMode2D.Impulse);
                jumpPressed = false;
            }
            

            Main.Anim.SetBool(Jumping,!(isGrounded));
        
        }

        private void FixedUpdate()
        {
            float speed = Main.MovementSpeed;

            if (!isGrounded)
                speed *= airControlMultiplier;

            Main.Rb.linearVelocity = new Vector2(
                hInput * speed,
                Main.Rb.linearVelocity.y
            );
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
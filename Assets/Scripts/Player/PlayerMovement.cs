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
        protected override void Awake()
        {
            base.Awake();
            
            input = new PlayerInput();
            input.Gameplay.Enable();

            input.Gameplay.AD.performed += OnMovePerformed;
            input.Gameplay.AD.canceled += OnMoveCanceled;
            input.Gameplay.Jump.performed += OnJumpPerformed;
            
            groundMask = LayerMask.GetMask("Ground");
        }

        private void OnJumpPerformed(InputAction.CallbackContext obj)
        {
            jumpPressed = true;
        }

        private void OnMoveCanceled(InputAction.CallbackContext obj)
        {
            hInput = 0;
        }

        private void OnMovePerformed(InputAction.CallbackContext obj)
        {
            hInput = obj.ReadValue<float>();
        }

        void Update()
        {
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
            Main.Rb.AddForce(new Vector2(hInput, 0) * Main.MovementSpeed, ForceMode2D.Force);
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
using System;
using Managers;
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
        private float vInput;
        private int groundMask;
        private bool isGrounded;
        private bool jumpPressed;
        private bool canUseStairs;

        private PlayerInput input;
        [SerializeField] private float airControlMultiplier = 1.2f;

        protected override void Awake()
        {
            base.Awake();
            
            input = new PlayerInput();
            groundMask = LayerMask.GetMask("Ground");
        }

        private void OnEnable()
        {
            input.Enable();

            if (Main.SlowPlaying)
            {
                input.Terror.Interact.performed += InteractPerformed;
            }
            else
            {
                input.Bosque.Jump.performed += OnJumpPerformed;
                input.Bosque.Interact.performed += InteractPerformed;
            }
        }

        private void OnDisable()
        {
            if (Main.SlowPlaying)
            {
                input.Terror.Interact.performed -= InteractPerformed;
            }
            else
            {
                input.Bosque.Jump.performed -= OnJumpPerformed;
                input.Bosque.Interact.performed -= InteractPerformed;
            }

            input.Disable();
        }

        private void Start()
        {
            if (Main.SlowPlaying)
            {
                input.Terror.Enable();
                input.Bosque.Disable();
            }
            else
            {
                input.Bosque.Enable();
                input.Terror.Disable();
            }
        }

        private void InteractPerformed(InputAction.CallbackContext obj)
        {
            EventManager.Instance.OnRelease();
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

            if (canUseStairs)
            {
                vInput = input.Terror.WS.ReadValue<float>();
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

            Vector2 vel = Main.Rb.linearVelocity;

            vel.x = hInput * speed;

            if (canUseStairs)
                vel.y = vInput * speed;

            Main.Rb.linearVelocity = vel;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Stairs"))
            {
                canUseStairs = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Stairs"))
            {
                canUseStairs = false;
            }
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
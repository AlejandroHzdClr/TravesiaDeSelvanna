using UnityEngine;

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


        protected override void Awake()
        {
            base.Awake();
            groundMask = LayerMask.GetMask("Ground");
        }

        void Update()
        {
            hInput = Input.GetAxisRaw("Horizontal");

            if (hInput>0)
            {
                transform.eulerAngles = Vector3.zero;
            }
            else if(hInput < 0 && transform.eulerAngles.y==0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        
            Main.Anim.SetBool(Walking,hInput!=0);
        
            isGrounded = Physics2D.OverlapCircle(
                groundCheck.position,
                radius,
                groundMask
            );

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isGrounded)
                {
                    Main.Rb.AddForce(Vector2.up * Main.JumpForce, ForceMode2D.Impulse);
                }
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
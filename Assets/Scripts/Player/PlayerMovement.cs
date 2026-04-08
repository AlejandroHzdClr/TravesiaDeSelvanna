using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private static readonly int Jumping = Animator.StringToHash("Jumping");
    private static readonly int Walking = Animator.StringToHash("Walking");


    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float radius;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;

    private Rigidbody2D rb;
    private float hInput;
    private int groundMask;
    private bool isGrounded;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
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
        
        anim.SetBool(Walking,hInput!=0);
        
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            radius,
            groundMask
        );

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        anim.SetBool(Jumping,!(isGrounded));
        
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector2(hInput, 0) * movementSpeed, ForceMode2D.Force);
        
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
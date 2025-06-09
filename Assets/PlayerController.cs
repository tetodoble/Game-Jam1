using UnityEngine;

public class MegaManController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;
    public float fallMultiplier = 3f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator anim;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }

    void Update()
    {
        Move();
        HandleJumpInput();
        FallFaster();
        HandleAnimation();
    }

    void FixedUpdate()
    {
        CheckGround();
    }

    void Move()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void HandleJumpInput()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void FallFaster()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    void HandleAnimation()
    {
        // Define velocidade horizontal para transição Idle <-> Run
        anim.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));

        // Define se está pulando para ativar animação de pulo
        anim.SetBool("IsJumping", !isGrounded);
        Debug.Log("Speed: " + Mathf.Abs(rb.linearVelocity.x));

    }

}
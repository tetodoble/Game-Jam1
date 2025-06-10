using UnityEngine;
using System.Collections;

public class MegaManController : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;

    [Header("Pulo")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;
    public float fallMultiplier = 3f;

    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private Rigidbody2D rb;
    private Animator anim;

    private bool isGrounded;
    private bool isDashing = false;
    private bool canDash = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isDashing)
        {
            Move();
            HandleJumpInput();
            FallFaster();
            HandleAnimation();
        }

        if (Input.GetKeyDown(KeyCode.R) && canDash)
        {
            StartCoroutine(PerformDash());
        }
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

    void HandleAnimation()
    {
        anim.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        anim.SetBool("IsJumping", !isGrounded);
    }

    private IEnumerator PerformDash()
    {
        isDashing = true;
        canDash = false;

        // Ativa trigger no Animator
        anim.SetTrigger("DashTrigger");

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashSpeed, 0f);

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
<<<<<<< Updated upstream
}
=======

    void HandleAnimation()
    {
        // Define velocidade horizontal para transição Idle <-> Run
        anim.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));

        // Define se está pulando para ativar animação de pulo
        anim.SetBool("IsJumping", !isGrounded);
        Debug.Log("Speed: " + Mathf.Abs(rb.linearVelocity.x));

    }
}
>>>>>>> Stashed changes

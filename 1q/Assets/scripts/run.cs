using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))] // Solo los tres primeros componentes
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float jumpForce = 9f;

    //hola

    [Header("Chequeo de suelo")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.4f;
    public LayerMask groundLayer;

    [Header("Reinicio al caer")]
    public float fallLimitY = -10f;

    [Header("Doble Salto")]
    public float doubleJumpAnimationSpeed = 1.2f;

    [Header("Sonidos")]
    public AudioClip jumpSound;  // Sonido para el salto
    public AudioClip runSound;   // Sonido para correr
    public AudioClip fallSound;  // Sonido para la caída

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;  // Aquí es donde agregamos la referencia a AudioSource

    private bool isGrounded;
    private float moveInput;
    private bool jumpRequested;

    // Doble salto
    private bool canDoubleJump;
    private bool hasJumpedOnce;
    private bool isDoubleJumping;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();  // Asegúrate de agregar el AudioSource aquí
    }

    void Update()
    {
        moveInput = 0f;

        if (Keyboard.current.aKey.isPressed)
            moveInput = -1f;
        else if (Keyboard.current.dKey.isPressed)
            moveInput = 1f;

        // Voltear sprite
        if (moveInput != 0)
            spriteRenderer.flipX = moveInput < 0;

        // Saltar o doble salto
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (IsGrounded())
            {
                jumpRequested = true;
                canDoubleJump = true;
                hasJumpedOnce = false;
                isDoubleJumping = false;
                animator.SetTrigger("jump");

                // Reproducir sonido de salto
                audioSource.PlayOneShot(jumpSound);
            }
            else if (canDoubleJump && !hasJumpedOnce)
            {
                jumpRequested = true;
                hasJumpedOnce = true;
                isDoubleJumping = true;
                animator.speed = doubleJumpAnimationSpeed;
                animator.Play("doubleJump");

                // Reproducir sonido de salto
                audioSource.PlayOneShot(jumpSound);
            }
        }

        // Reinicio de escena si se cae
        if (transform.position.y < fallLimitY)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        UpdateAnimations(moveInput);
    }

    void FixedUpdate()
    {
        // Movimiento horizontal
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Saltar
        if (jumpRequested)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpRequested = false;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void UpdateAnimations(float moveInput)
    {
        isGrounded = IsGrounded();

        if (isGrounded)
        {
            isDoubleJumping = false;
            animator.speed = 1f; // Reiniciar velocidad normal

            if (Mathf.Abs(moveInput) > 0.1f)
            {
                animator.Play("run");

                // Reproducir sonido de correr
                if (!audioSource.isPlaying)
                    audioSource.PlayOneShot(runSound);
            }
            else
            {
                animator.Play("idle");
            }
        }
        else
        {
            if (isDoubleJumping)
            {
                // Mantener animación de doble salto hasta que empiece a caer
                if (rb.linearVelocity.y < -0.1f)
                    animator.Play("fall");
            }
            else
            {
                if (rb.linearVelocity.y > 0.1f)
                    animator.Play("jump");
                else if (rb.linearVelocity.y < -0.1f)
                    animator.Play("fall");

                // Reproducir sonido de caída solo si no está saltando
                if (rb.linearVelocity.y < -0.1f && !audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(fallSound);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        moveInput = Vector2.zero;

        if (Keyboard.current == null) return;

        if (Keyboard.current.wKey.isPressed)
            moveInput.y += 1;

        if (Keyboard.current.sKey.isPressed)
            moveInput.y -= 1;

        if (Keyboard.current.aKey.isPressed)
            moveInput.x -= 1;

        if (Keyboard.current.dKey.isPressed)
            moveInput.x += 1;

        moveInput = moveInput.normalized;

        // アニメーション
        if (animator != null)
            animator.SetFloat("Speed", moveInput.magnitude);

        // キャラクターの左右反転
        if (spriteRenderer != null)
        {
            if (moveInput.x < 0)
                spriteRenderer.flipX = true;
            else if (moveInput.x > 0)
                spriteRenderer.flipX = false;
        }
    }

    void FixedUpdate()
    {
        if (rb != null)
            rb.linearVelocity = moveInput * moveSpeed;
    }
}
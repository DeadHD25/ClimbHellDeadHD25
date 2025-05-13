using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float jump;
    public float speed;
    public float dashSpeed;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private Rigidbody2D rb;
    private bool grounded;
    private bool isDashing;
    private float dashTimer;
    private float dashCooldownTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Handle cooldown timer
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        // Dashing logic
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                isDashing = false;
            }
            return; // Skip movement and jump while dashing
        }

        // Horizontal movement
        Vector3 playerInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
        transform.position = transform.position + playerInput.normalized * speed * Time.deltaTime;

        // Jump
        if (Input.GetButtonDown("Jump") && grounded)
        {
            rb.AddForce(new Vector2(rb.velocity.x, jump));
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0 && playerInput.x != 0)
        {
            StartCoroutine(Dash(playerInput.x));
        }
    }

    IEnumerator Dash(float direction)
    {
        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(direction * dashSpeed, 0);

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        rb.velocity = Vector2.zero;
        isDashing = false;
    }

    // Ground check
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Vector3 normal = other.GetContact(0).normal;
            if (normal == Vector3.up)
            {
                grounded = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }
}

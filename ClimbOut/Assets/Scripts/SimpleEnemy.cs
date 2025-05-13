using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 3f;
    public float jumpForce = 300f;
    public Transform player;

    private Rigidbody2D rb;
    private bool grounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (player == null && GameObject.FindGameObjectWithTag("Player"))
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
{
    if (player == null) return;

    Vector2 direction = (player.position - transform.position).normalized;

    // Move left or right only
    rb.velocity = new Vector2(Mathf.Sign(direction.x) * speed, rb.velocity.y);

    // Jump if grounded and player is above
    bool shouldJump = grounded && player.position.y > transform.position.y + 1f;

    // OR if grounded and blocked by wall (obstacle ahead)
    if (grounded && IsBlockedAhead())
    {
        shouldJump = true;
    }

    if (shouldJump)
    {
        rb.AddForce(new Vector2(0, jumpForce));
    }

    bool IsBlockedAhead()
{
    float directionX = Mathf.Sign(player.position.x - transform.position.x);
    Vector2 origin = new Vector2(transform.position.x, transform.position.y);
    Vector2 dir = new Vector2(directionX, 0);

    RaycastHit2D hit = Physics2D.Raycast(origin, dir, 0.5f, LayerMask.GetMask("Ground"));
    Debug.DrawRay(origin, dir * 0.5f, Color.red); // Optional: see it in Scene view

    return hit.collider != null;
}

}


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

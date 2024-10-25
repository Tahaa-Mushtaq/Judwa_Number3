using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    public float speed = 5f; // Movement speed
    public float jumpForce = 4f; // Jump force
    private Rigidbody2D rb;
    private int jumpCount = 0;
    private int maxJumps = 2; // Max jumps allowed

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Horizontal movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal * speed, rb.velocity.y);
        rb.velocity = movement;

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            jumpCount++;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            jumpCount = 0;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    public float speed = 5f; // Movement speed
    public float jumpForce = 7f; // Jump force
    public GameObject playerPrefab; // Prefab for the player

    private Rigidbody2D rb;
    private int jumpCount = 0;
    private int maxJumps = 2; // Max jumps allowed
    private bool canClone = true; // Only original can clone

    private static List<GameObject> clones = new List<GameObject>(); // List to track clones

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component missing from this GameObject!");
        }
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

        // Clone creation
        if (Input.GetKeyDown(KeyCode.Z) && canClone)
        {
            if (playerPrefab != null) // Check if playerPrefab is assigned
            {
                Vector2 spawnPosition = new Vector2(transform.position.x + 1, transform.position.y); // Adjust spawn position as needed
                GameObject clone = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

                // Ensure the clone cannot create more clones
                Movement2D cloneScript = clone.GetComponent<Movement2D>();
                if (cloneScript != null)
                {
                    cloneScript.playerPrefab = playerPrefab;
                    cloneScript.canClone = false; // Disable cloning for the clone
                    clones.Add(clone); // Add clone to the list
                }
                else
                {
                    Debug.LogError("Movement2D component missing from playerPrefab!");
                }
            }
            else
            {
                Debug.LogError("playerPrefab is not assigned in the Inspector!");
            }
        }

        // Freeze one clone on pressing 'X'
        if (Input.GetKeyDown(KeyCode.X))
        {
            FreezeClone();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            jumpCount = 0;
        }
    }

    private void FreezeClone()
    {
        foreach (GameObject clone in clones)
        {
            // Check if the clone's Rigidbody2D is active (not frozen)
            Rigidbody2D cloneRb = clone.GetComponent<Rigidbody2D>();
            if (cloneRb != null && cloneRb.isKinematic == false)
            {
                cloneRb.isKinematic = true; // Freeze the clone by making it kinematic
                Debug.Log("Clone frozen at position: " + clone.transform.position);
                return; // Stop after freezing the first non-frozen clone
            }
        }
        Debug.Log("All clones are already frozen or none created.");
    }
}

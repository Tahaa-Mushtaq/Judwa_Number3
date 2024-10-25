using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    public float speed = 5f; // Movement speed
    public GameObject playerPrefab; // Prefab for the player
    private Rigidbody2D rb;
    private Animator animator;
    private List<GameObject> clones = new List<GameObject>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    void Update()
    {
        // Horizontal movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal * speed, rb.velocity.y);
        rb.velocity = movement;

        // Set animation parameter for running based on horizontal movement
        animator.SetFloat("Speed", Mathf.Abs(moveHorizontal));

        // Clone creation with limit of 3 clones
        if (Input.GetKeyDown(KeyCode.Z) && clones.Count < 3)
        {
            if (playerPrefab != null) // Check if playerPrefab is assigned
            {
                Vector2 spawnPosition = new Vector2(transform.position.x + 1, transform.position.y); // Adjust spawn position as needed
                GameObject clone = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
                clone.GetComponent<Movement2D>().playerPrefab = playerPrefab; // Ensure the clone can create more clones too
                clones.Add(clone);
            }
            else
            {
                Debug.LogError("playerPrefab is not assigned in the Inspector!");
            }
        }

        // Freeze clones one by one when pressing 'X'
        if (Input.GetKeyDown(KeyCode.X))
        {
            FreezeClone();
        }
    }

    private void FreezeClone()
    {
        for (int i = 0; i < clones.Count; i++)
        {
            GameObject clone = clones[i];
            Movement2D cloneScript = clone.GetComponent<Movement2D>();
            Rigidbody2D cloneRb = clone.GetComponent<Rigidbody2D>();

            if (cloneScript != null && cloneScript.enabled && cloneRb != null)
            {
                // Freeze the clone by making it kinematic and stopping script activity
                cloneScript.enabled = false; // Disable the script to stop all movement
                cloneRb.isKinematic = true;  // Make kinematic to prevent physics interference
                cloneRb.mass = 1000f; // Increase mass to prevent movement on collisions
                clone.layer = LayerMask.NameToLayer("FrozenClone"); // Assign to a non-interacting layer

                // Remove from the clones list
                clones.RemoveAt(i);
                Debug.Log("Clone frozen at position: " + clone.transform.position);
                return; // Exit after freezing the first unfrozen clone
            }
        }
        Debug.Log("All clones are already frozen or none created.");
    }
}

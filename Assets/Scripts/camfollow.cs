using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform; // The specified player object to follow
    public Vector3 offset; // Offset distance between the player and camera

    void Start()
    {
        // Initialize offset if not set in the Inspector
        if (offset == Vector3.zero)
        {
            offset = transform.position - playerTransform.position;
        }
    }

    void LateUpdate()
    {
        // Update camera position to follow the player
        transform.position = playerTransform.position + offset;
    }
}

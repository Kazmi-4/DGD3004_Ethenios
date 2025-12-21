using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Header("Camera")]
    public Transform cam;   // Assign the Main Camera

    [Header("Parallax Strength")]
    [Range(0f, 1f)] public float parallaxX = 0.3f;   // Horizontal movement
    [Range(0f, 1f)] public float parallaxY = 0.0f;   // Vertical movement
    [Range(0f, 1f)] public float parallaxZ = 0.0f;   // Depth movement

    [Header("Axis Toggles")]
    public bool allowYMovement = false;   // If off, Y stays locked
    public bool allowZMovement = false;   // If off, Z stays locked

    float startX, startY, startZ;

    void Start()
    {
        if (cam == null)
            cam = Camera.main.transform;

        Vector3 startPos = transform.position;
        startX = startPos.x;
        startY = startPos.y;
        startZ = startPos.z;
    }

    void LateUpdate()
    {
        // --- X movement (always enabled)
        float newX = startX + cam.position.x * parallaxX;

        // --- Y movement (optional)
        float newY = allowYMovement
            ? startY + cam.position.y * parallaxY
            : startY;

        // --- Z movement (optional)
        float newZ = allowZMovement
            ? startZ + cam.position.z * parallaxZ
            : startZ;

        transform.position = new Vector3(newX, newY, newZ);
    }
}

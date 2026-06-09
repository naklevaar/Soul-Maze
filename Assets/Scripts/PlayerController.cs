using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Configuration")]
    [Tooltip("Controls the speed and tilt responsiveness of the ball.")]
    [SerializeField] private float movementSensitivity = 15f;

    private Rigidbody rb;
    private Vector3 movementDirection;

    private void Start()
    {
        // Cache the Rigidbody component for optimal performance
        rb = GetComponent<Rigidbody>();

        // Ensure the ball reacts properly to gravity in the 3D maze
        rb.useGravity = true;
    }

    private void Update()
    {
        // 1. Keyboard Input (For seamless testing inside the Unity Editor)
        float keyboardX = Input.GetAxis("Horizontal"); // A/D or Left/Right arrows
        float keyboardZ = Input.GetAxis("Vertical");   // W/S or Up/Down arrows

        // Default movement direction based on keyboard
        movementDirection = new Vector3(keyboardX, 0f, keyboardZ);

        // 2. Accelerometer Input (For the final Android mobile build)
        if (SystemInfo.supportsAccelerometer)
        {
            // When holding a mobile device flat:
            // Input.acceleration.x tracks left/right tilt (X axis)
            // Input.acceleration.y tracks forward/backward tilt (mapped to gameplay Z axis)
            float tiltX = Input.acceleration.x;
            float tiltZ = Input.acceleration.y;

            // Deadzone check to prevent tiny accidental shaking/drifting
            if (Mathf.Abs(tiltX) > 0.05f || Mathf.Abs(tiltZ) > 0.05f)
            {
                movementDirection = new Vector3(tiltX, 0f, tiltZ);
            }
        }
    }

    private void FixedUpdate()
    {
        // All physics calculations and force applications must happen in FixedUpdate
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (movementDirection != Vector3.zero)
        {
            // Apply a continuous force to the Rigidbody based on input and sensitivity
            rb.AddForce(movementDirection * movementSensitivity, ForceMode.Force);
        }
    }

    /// <summary>
    /// Allows external scripts (like a UI Options Menu Slider) to adjust sensitivity dynamically.
    /// </summary>
    public void UpdateSensitivity(float newSensitivity)
    {
        movementSensitivity = newSensitivity;
    }
}
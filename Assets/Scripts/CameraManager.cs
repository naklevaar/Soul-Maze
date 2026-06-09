using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera References")]
    [SerializeField] private Camera followCamera;
    [SerializeField] private Camera topDownCamera;

    [Header("Follow Camera Settings")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector3 followOffset = new Vector3(0f, 5f, -7f);
    [SerializeField] private float smoothSpeed = 5f;

    private bool isTopDownMode = false;

    private void Start()
    {
        // Safety check to ensure references are assigned
        if (followCamera == null || topDownCamera == null || playerTransform == null)
        {
            Debug.LogError("CameraManager: Please assign all references in the Inspector!");
            return;
        }

        // Start the game in Follow Camera mode
        SetCameraMode(false);
    }

    private void Update()
    {
        // Switch cameras when the 'C' key is pressed (for Editor testing)
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCamera();
        }
    }

    private void LateUpdate()
    {
        // Camera movement physics should always run in LateUpdate after the player has moved
        if (!isTopDownMode && playerTransform != null)
        {
            HandleFollowCamera();
        }
    }

    /// <summary>
    /// Smoothly glides the follow camera behind the player sphere.
    /// </summary>
    private void HandleFollowCamera()
    {
        Vector3 targetPosition = playerTransform.position + followOffset;
        // Smoothly interpolate between the camera's current position and the target position
        followCamera.transform.position = Vector3.Lerp(followCamera.transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // Keep the camera pointing directly at the rolling Spirit Orb
        followCamera.transform.LookAt(playerTransform.position);
    }

    public void ToggleCamera()
    {
        SetCameraMode(!isTopDownMode);
    }

    private void SetCameraMode(bool topDownActive)
    {
        isTopDownMode = topDownActive;

        // Enable one camera and disable the other
        topDownCamera.gameObject.SetActive(isTopDownMode);
        followCamera.gameObject.SetActive(!isTopDownMode);
    }
}
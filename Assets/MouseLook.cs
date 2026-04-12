using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Sensitivity")]
    public float sensitivityX = 2.0f;
    public float sensitivityY = 2.0f;

    [Header("Vertical clamp (degrees)")]
    public float minPitch = -90f;
    public float maxPitch =  90f;

    [Header("References")]
    public Transform playerBody;   // drag your Player GameObject here

    private float xRotation = 0f;  // tracks cumulative vertical look angle

    void Start()
    {
        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Read raw mouse delta this frame
        float mouseX = Input.GetAxis("Mouse X") * sensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY;

        // Vertical: accumulate and clamp, then apply to camera
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minPitch, maxPitch);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal: rotate the whole player body
        playerBody.Rotate(Vector3.up * mouseX);
    }
}

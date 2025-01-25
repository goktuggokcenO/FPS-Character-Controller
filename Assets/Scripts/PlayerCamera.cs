using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Sensitivity")]
    public float xSensitivity;
    public float ySensitivity;
    public Transform orientation;

    float xRotation;
    float yRotation;

    private void Start()
    {
        // Lock the crusor and set the visibility to false.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Calculate the x and y value and rortation for each axis.
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * xSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * xSensitivity;

        yRotation += mouseX;
        xRotation -= mouseY;

        // Clamp the camera angle for x to -90 to 90 degree so camera can't spin vertically.s
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Rotate the character body with camera.
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}

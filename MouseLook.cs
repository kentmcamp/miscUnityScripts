using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 1.5f;
    public float smoothing = 1.5f;

    private float xMousePos;
    private float yMousePos;

    private float smoothXMousePos;
    private float smoothYMousePos;

    private float currentXLookPos;
    private float currentYLookPos;

    public float clampAngle = 80f;

    void Start()
    {
        // Lock and Hide Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        ModifyInput();
        RotateCamera();
    }

    private void GetInput()
    {
        xMousePos = Input.GetAxisRaw("Mouse X");
        yMousePos = Input.GetAxisRaw("Mouse Y");
    }

    private void ModifyInput()
    {
        xMousePos *= sensitivity * smoothing;
        yMousePos *= sensitivity * smoothing;

        smoothXMousePos = Mathf.Lerp(smoothXMousePos, xMousePos, 1f / smoothing);
        smoothYMousePos = Mathf.Lerp(smoothYMousePos, yMousePos, 1f / smoothing);
    }

    private void RotateCamera()
    {
        currentXLookPos += smoothXMousePos;
        currentYLookPos -= smoothYMousePos; // Invert Y axis for natural feel

        // Clamp the vertical rotation to prevent flipping
        currentYLookPos = Mathf.Clamp(currentYLookPos, -clampAngle, clampAngle);

        transform.localRotation = Quaternion.Euler(currentYLookPos, 0f, 0f);
        transform.parent.rotation = Quaternion.Euler(0f, currentXLookPos, 0f);
    }
}
using UnityEngine;

public class SimpleCamera : MonoBehaviour
{
    public float sensitivity = 10;
    public float normalMoveSpeed = 15;
    public float slowFactor = 0.5f;
    public float fastFactor = 10;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    private void Update()
    {
        // handle rotation
        if (Input.GetMouseButton(1))
        {
            rotationX += Input.GetAxis("Mouse X") * sensitivity;
            rotationY += Input.GetAxis("Mouse Y") * sensitivity;
            rotationY = Mathf.Clamp(rotationY, -90, 90);
        }

        // apply rotation
        transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

        // speed multiplier
        float speed = normalMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            speed *= fastFactor;
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            speed *= slowFactor;

        // apply movement
        transform.position += transform.forward * speed * Input.GetAxis("Vertical");
        transform.position += transform.right * speed * Input.GetAxis("Horizontal");

        // up/down
        if (Input.GetKey(KeyCode.Q))
            transform.position += Vector3.up * speed;
        if (Input.GetKey(KeyCode.E))
            transform.position -= Vector3.up * speed;
    }
}

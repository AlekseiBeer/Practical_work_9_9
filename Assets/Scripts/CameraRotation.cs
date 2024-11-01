using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float horizontalRotationSpeed = 100f;
    public float verticalRotationSpeed = 50f;

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(Vector3.down * horizontalRotationSpeed * Time.deltaTime, Space.World);

        if (Input.GetKey(KeyCode.D))
            transform.Rotate(Vector3.up * horizontalRotationSpeed * Time.deltaTime, Space.World);

        if (Input.GetKey(KeyCode.W))
            transform.Rotate(Vector3.left * verticalRotationSpeed * Time.deltaTime, Space.Self);

        if (Input.GetKey(KeyCode.S))
            transform.Rotate(Vector3.right * verticalRotationSpeed * Time.deltaTime, Space.Self);
    }
}

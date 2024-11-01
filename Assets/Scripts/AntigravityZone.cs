using UnityEngine;

public class AntigravityZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null)
            rb.useGravity = false;
    }

    void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null)
            rb.useGravity = true;
    }
}

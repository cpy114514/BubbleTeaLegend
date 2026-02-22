using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;     // Player
    public float smoothTime = 0.15f;

    Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPos = target.position;
        targetPos.z = transform.position.z; // ±£³Ö Camera Z

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            smoothTime
        );
    }
}

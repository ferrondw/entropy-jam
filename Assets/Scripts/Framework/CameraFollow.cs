using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float smoothTime;
    public Transform target;
    private Camera cam;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        Vector3 point = cam.WorldToViewportPoint(target.position);
        Vector3 delta = target.position - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
        Vector3 destination = transform.position + delta;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, smoothTime);
    }
}

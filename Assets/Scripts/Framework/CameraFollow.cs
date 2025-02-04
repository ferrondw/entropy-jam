using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;
    
    public float SmoothTime;
    public Transform Target;
    private Camera _cam;
    private Vector3 _velocity = Vector3.zero;
    
    private Vector3 shakeOffset = Vector3.zero;
    private float _timeAtCurrentFrame;
    private float _timeAtLastFrame;
    private float _fakeDelta;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _cam = GetComponent<Camera>();
    }

    private void Update()
    {
        _timeAtCurrentFrame = Time.realtimeSinceStartup;
        _fakeDelta = _timeAtCurrentFrame - _timeAtLastFrame;
        _timeAtLastFrame = _timeAtCurrentFrame;
    }

    private void LateUpdate()
    {
        if (Target == null) return;
        
        Vector3 delta = Target.position - _cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10));
        Vector3 destination = transform.position + delta;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref _velocity, SmoothTime);
        transform.position += shakeOffset;
    }

    public static void Shake(float duration, float amount)
    {
        instance.StopAllCoroutines();
        instance.StartCoroutine(instance.cShake(duration, amount));
    }

    private IEnumerator cShake(float duration, float amount)
    {
        float endTime = Time.time + duration;

        while (duration > 0)
        {
            shakeOffset = Random.insideUnitSphere * amount;
            duration -= _fakeDelta;
            yield return null;
        }

        shakeOffset = Vector3.zero;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class CursorFollow : MonoBehaviour
{
    [SerializeField] private Transform dragTarget;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite dragSprite;
    [SerializeField] private float smoothTime;
    [SerializeField] private Gradient stressColor;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotationVelocity = Vector3.zero;

    private void Start()
    {
        spriteRenderer.sprite = defaultSprite;
    }

    private void Update()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0))
        {
            spriteRenderer.sprite = dragSprite;
            Vector3 diff = worldPosition - dragTarget.position;
            var absoluteDiff = diff;
            diff.Normalize();
            float rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = SmoothDampQuaternion(transform.rotation, Quaternion.Euler(0f, 0f, rotZ - 270), ref rotationVelocity, smoothTime);
            
            // shake
            var amount = Mathf.Clamp(Mathf.Clamp(absoluteDiff.magnitude, 0, 20) / 30 - 0.2f, 0, 5);
            var offsetX = Random.Range(-amount, amount);
            var offsetY = Random.Range(-amount, amount);
            spriteRenderer.transform.localPosition = new Vector3(offsetX, offsetY, 0);
            spriteRenderer.color = stressColor.Evaluate(amount * 7);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            spriteRenderer.transform.localPosition = Vector3.zero;
            spriteRenderer.sprite = defaultSprite;
            spriteRenderer.color = Color.white;
        }
        else
        {
            transform.rotation = SmoothDampQuaternion(transform.rotation, Quaternion.identity, ref rotationVelocity, smoothTime);
        }

        Vector3 delta = new Vector3(worldPosition.x, worldPosition.y, -5);
        transform.position = Vector3.SmoothDamp(transform.position, delta, ref velocity, smoothTime);
    }

    private static Quaternion SmoothDampQuaternion(Quaternion current, Quaternion target, ref Vector3 currentVelocity, float smoothTime)
    {
        Vector3 c = current.eulerAngles;
        Vector3 t = target.eulerAngles;
        return Quaternion.Euler(
            Mathf.SmoothDampAngle(c.x, t.x, ref currentVelocity.x, smoothTime),
            Mathf.SmoothDampAngle(c.y, t.y, ref currentVelocity.y, smoothTime),
            Mathf.SmoothDampAngle(c.z, t.z, ref currentVelocity.z, smoothTime)
        );
    }
}
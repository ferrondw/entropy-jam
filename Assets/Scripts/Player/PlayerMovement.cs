using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    public static bool canShoot = true;
    public static bool hasBashed = true;
 
    [SerializeField] private Camera camera;
    [SerializeField] private Transform cursor;
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int numberOfPoints;
    [SerializeField] private float timeBetweenPoints;
    [SerializeField] private float forceMultiplier;
    [SerializeField] private float velocityThreshold;
    [SerializeField] private TMP_Text shotsText;
    [SerializeField] private Gradient stressColor;
    [SerializeField] private Rigidbody2D ballBody;

    [Space(20)] [SerializeField] private UnityEvent onPutt;
    [SerializeField] private UnityEvent onLand;
    
    private Vector2 launchVelocity;
    private bool isTouchingGround;
    private bool inWater;
    private int shots;

    private void Start()
    {
        lineRenderer.positionCount = numberOfPoints;
        Cursor.visible = false;
        canShoot = true;
        shots = 0;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        Cursor.visible = hasFocus;
    }

    private void Update()
    {
        ballBody.transform.position = transform.position;
        ballBody.rotation += -rigidbody.velocity.x * Time.deltaTime * 50f;
        
        float targetSize = 7 + rigidbody.velocity.magnitude * 0.2f;
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetSize, Time.deltaTime * 2);

        canShoot = isTouchingGround && rigidbody.velocity.magnitude < velocityThreshold;
        
        if (canShoot && Input.GetMouseButton(0))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = 0;

            Vector2 force = (transform.position - worldPosition) * forceMultiplier;

            launchVelocity = force / rigidbody.mass;
            DrawArc(launchVelocity);
        }
        else if (canShoot && Input.GetMouseButtonUp(0))
        {
            ShootBall();
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    private void DrawArc(Vector2 initialVelocity)
    {
        Vector3 diff = transform.position - cursor.position;
        var amount = Mathf.Clamp(Mathf.Clamp(diff.magnitude, 0, 20) / 30 - 0.2f, 0, 5);
        lineRenderer.startColor = stressColor.Evaluate(amount * 7);
        var col = stressColor.Evaluate(amount * 7);
        lineRenderer.endColor = new Color(col.r, col.g, col.b, 0);
        
        lineRenderer.enabled = true;
        Vector2 currentPosition = transform.position;
        Vector2 currentVelocity = initialVelocity;

        for (int i = 0; i < numberOfPoints; i++)
        {
            lineRenderer.SetPosition(i, currentPosition);

            currentPosition += currentVelocity * timeBetweenPoints;
            currentVelocity += Physics2D.gravity * (timeBetweenPoints * (inWater ? -1 : 1));
        }
    }

    private void ShootBall()
    {
        rigidbody.velocity = launchVelocity;
        canShoot = false;
        lineRenderer.enabled = false;
        shots++;
        shotsText.text = $"Shots: {shots}";
        shotsText.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
        shotsText.transform.DOScale(Vector3.one, 0.4f);
        onPutt.Invoke();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var collisionTransform = other.transform;
        float distanceY = transform.position.y - collisionTransform.position.y - collisionTransform.lossyScale.y / 2;
        Debug.Log(distanceY);
        if (!(distanceY > 0)) return; // on top of platform
        
        onLand.Invoke();
        Debug.Log("Landed on top of platform");
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Ground")) return;

        var position = transform.position;
        var otherPosition = other.transform.position;
        var otherHalfHeight = other.transform.localScale.y / 2;

        bool onTop = position.y - otherPosition.y >= otherHalfHeight;
        bool onWaterCeiling = position.y - otherPosition.y <= -otherHalfHeight && inWater;

        if (!onTop && !onWaterCeiling) return;

        isTouchingGround = true;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Ground")) return;
        isTouchingGround = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Death")) return;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Water")) return;
        inWater = true;
        var targetVelocityY = Mathf.Min(rigidbody.velocity.y + 14 * Time.deltaTime, 20);
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, targetVelocityY);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Water")) return;
        inWater = false;
    }
}
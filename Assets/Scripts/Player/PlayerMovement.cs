using UnityEngine;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;
using UnityEditor;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int numberOfPoints;
    [SerializeField] private float timeBetweenPoints;
    [SerializeField] private float forceMultiplier;
    [SerializeField] private float velocityThreshold;
    [SerializeField] private TMP_Text shotsText;
    [Space(20)] [SerializeField] private UnityEvent onPutt;
    [SerializeField] private UnityEvent onLand;

    private Vector2 launchVelocity;
    private bool isTouchingGround;
    private bool canShoot = true;
    private bool inWater;
    private int shots;

    private void Start()
    {
        lineRenderer.positionCount = numberOfPoints;
        shots = 0;
    }

    private void Update()
    {
        float targetSize = 7 + (rigidbody.velocity.magnitude * 0.2f);
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetSize, Time.deltaTime * 2);

        canShoot = isTouchingGround && rigidbody.velocity.magnitude < velocityThreshold;

        if (canShoot && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = camera.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0;

            Vector2 force = (transform.position - touchPosition) * forceMultiplier;

            launchVelocity = force / rigidbody.mass;
            DrawArc(launchVelocity);

            if (touch.phase == TouchPhase.Ended)
            {
                ShootBall();
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    private void DrawArc(Vector2 initialVelocity)
    {
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
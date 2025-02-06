using UnityEngine;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Yakanashe.Wiper;

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
    [SerializeField] private TMP_Text shotsText;
    [SerializeField] private Gradient stressColor;
    [SerializeField] private Rigidbody2D ballBody;
    [SerializeField] private float exitTime = 0.2f;
    [SerializeField] private Transition wiper;

    [Space(20)] [SerializeField] private UnityEvent onPutt;
    [SerializeField] private UnityEvent onLand;
    public UnityEvent onBump;
    
    private Vector2 launchVelocity;
    private bool isTouchingGround;
    private bool inWater;

    private void Start()
    {
        lineRenderer.positionCount = numberOfPoints;
        Cursor.visible = false;
        canShoot = true;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        Cursor.visible = hasFocus;
    }

    private void Update()
    {
        var velocity = rigidbody.velocity;
        var targetSize = 7 + velocity.magnitude * 0.2f;

        if (Input.GetKeyDown(KeyCode.R))
        {
            wiper.In(0, () =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            });
        }
        
        ballBody.transform.position = transform.position;
        ballBody.rotation += -velocity.x * Time.deltaTime * 50f;
        
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetSize, Time.deltaTime * 2);

        canShoot = isTouchingGround;
        
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
        isTouchingGround = false;
        CameraFollow.Shake(0.2f, 0.3f);
        lineRenderer.enabled = false;
        
        if (Timer.instance)
        {
            Timer.instance.shotCount++;
            shotsText.text = $"Shots: {Timer.instance.shotCount}";
        }

        shotsText.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
        shotsText.transform.DOScale(Vector3.one, 0.4f);
        onPutt.Invoke();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var collisionTransform = other.transform;
        float distanceY = transform.position.y - collisionTransform.position.y - collisionTransform.lossyScale.y / 2;
        if (!(distanceY > 0)) return; // on top of platform
        onLand.Invoke();
        
        CancelInvoke(nameof(ExitFloor));
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
        Invoke(nameof(ExitFloor), exitTime);
    }
    
    private void ExitFloor() { isTouchingGround = false; }

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
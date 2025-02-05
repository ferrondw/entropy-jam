using UnityEngine;
using UnityEngine.UI;

public class FloatingUI : MonoBehaviour
{
    public float FloatSpeed;
    public float FloatAmount;
    public float RotationSpeed;

    private RectTransform rectTransform;
    private Vector3 startPos;
    private float randomOffset;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
        randomOffset = Random.Range(0f, Mathf.PI * 2);
    }

    private void Update()
    {
        var newY = startPos.y + Mathf.Sin(Time.time * FloatSpeed + randomOffset) * FloatAmount;
        rectTransform.anchoredPosition = new Vector2(startPos.x, newY);

        var newRot = Mathf.Sin(Time.time * FloatSpeed + randomOffset) * RotationSpeed;
        rectTransform.localRotation = Quaternion.Euler(0, 0, newRot);
    }
}
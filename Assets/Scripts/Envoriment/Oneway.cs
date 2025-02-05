using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D), typeof(PlatformEffector2D))]
public class Oneway : MonoBehaviour
{
    [SerializeField] [Range(1f, 200f)] private float size = 1f;
    
    [Header("Sprite Info")]
    [SerializeField] private Transform edgeLeft;
    [SerializeField] private Transform edgeRight;
    [SerializeField] private SpriteRenderer platform;
    
    [SerializeField] private float edgeMargin = 0.7f;
    [SerializeField] private float platformSize = 5.5f;

    private void OnValidate()
    {
        var edgingCollider = gameObject.GetComponent<EdgeCollider2D>();
        var edgingPoint = new Vector2(size / 2 + edgeMargin, 0);
        edgingCollider.points = new[] { -edgingPoint, edgingPoint };

        platform.drawMode = SpriteDrawMode.Sliced;
        platform.size = new Vector2(size, 1.5f);

        var edgePos = platform.localBounds.size.x / 2;
        if ((int)size == 1) edgePos = edgeMargin;
        
        edgeLeft.localPosition = new Vector3(-edgePos, 0, 0);
        edgeRight.localPosition = new Vector3(edgePos, 0, 0);
    }
}

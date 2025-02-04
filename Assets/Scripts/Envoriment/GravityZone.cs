using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class GravityZone : MonoBehaviour
{
    private readonly List<Rigidbody2D> bodies = new();
    
    [SerializeField] private Vector2 size = Vector2.one;
    public Vector2 Size { get => size; set { size = value; ScaleRect(); } }

    [SerializeField] private BoxCollider2D trigger;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Light2D lightBox;
    
    private void FixedUpdate()
    {
        ApplyGravity();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var rb = other.transform.GetComponent<Rigidbody2D>();
        if (rb == null) return;
        
        rb.gravityScale = 0;
        bodies.Add(rb);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var rb = other.transform.GetComponent<Rigidbody2D>();
        if (rb == null) return;
        
        rb.gravityScale = 1;
        bodies.Remove(rb);
    }

    private void ApplyGravity()
    {
        foreach (var body in bodies)
        {
            body.AddForce(transform.up * -Physics2D.gravity, ForceMode2D.Force);
        }
    }


    private void OnValidate() { ScaleRect(); }
    private void ScaleRect()
    {
        trigger.size = size;
        sprite.size = size;

        lightBox.lightType = Light2D.LightType.Freeform;
        var lightShape = new Vector3[4];
        lightShape[0] = (new Vector3(size.x / 2, size.y / 2));
        lightShape[1] = new Vector3(size.x / 2, -(size.y / 2));
        lightShape[2] = new Vector3(-(size.x / 2), -(size.y / 2));
        lightShape[3] = new Vector3(-(size.x / 2), size.y / 2);

        lightBox.SetShapePath(lightShape);
    }
}
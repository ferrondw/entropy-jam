using System;
using System.Collections.Generic;
using UnityEngine;

public class GravityZone : MonoBehaviour
{
    private readonly List<Rigidbody2D> bodies = new();

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
}
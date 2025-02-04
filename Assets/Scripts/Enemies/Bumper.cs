using System;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    public float forceMultiplier;

    private void OnCollisionEnter2D(Collision2D other)
    {
        Rigidbody2D otherRigidbody = other.rigidbody;
        if (otherRigidbody == null) return;
        
        Vector3 direction = (other.transform.position - transform.position).normalized;
        Vector3 force = direction * forceMultiplier;
        otherRigidbody.AddForce(force, ForceMode2D.Impulse);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Speedometer : MonoBehaviour
{
    private Rigidbody2D target;
    private TMP_Text text;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        text.text = Mathf.Round(target.velocity.magnitude * 4f) + " KM/h";
    }
}

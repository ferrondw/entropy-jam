using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Yakanashe.Wiper;

public class TimerText : MonoBehaviour
{
    public TMP_Text text;

    private void Start()
    {
        FindObjectOfType<Transition>().Out();
    }

    private void Update()
    {
        if (!text) return;
        text.text = Timer.instance.TimeString();
    }
}

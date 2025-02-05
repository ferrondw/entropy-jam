using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerText : MonoBehaviour
{
    public TMP_Text text;

    private void Update()
    {
        text.text = Timer.instance.TimeString();
    }
}

using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
            return;
        }
        
        Destroy(this);
    }
    
    private float startTime;

    public string TimeString()
    {
        var timePlaying = TimeSpan.FromSeconds(Time.time - startTime);
        return $"{timePlaying:mm':'ss'.'fff}";
    }

    public void ResetTimer()
    {
        startTime = Time.time;
    }
}

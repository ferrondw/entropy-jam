using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MenuAnimationSequence : MonoBehaviour
{
    public Transform Hamster, Geddon;
    public ParticleSystem backgroundParticles;

    private void Start()
    {
        Hamster.GetComponent<CanvasGroup>().DOFade(1, 1f);
        Geddon.GetComponent<CanvasGroup>().DOFade(1, 1f);
        Invoke(nameof(PlayParticles), 1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (Cursor.lockState == CursorLockMode.Confined)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
            }
        }
    }

    private void PlayParticles()
    {
        backgroundParticles.Play();
        CameraFollow.Shake(0.5f, 1);
    }
}

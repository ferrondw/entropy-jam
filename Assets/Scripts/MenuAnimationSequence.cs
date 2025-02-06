using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MenuAnimationSequence : MonoBehaviour
{
    public Transform Hamster, Geddon, SpikeBall;
    public ParticleSystem backgroundParticles;

    private void Start()
    {
        Hamster.GetComponent<CanvasGroup>().DOFade(1, 1f);
        Geddon.GetComponent<CanvasGroup>().DOFade(1, 1f);
        Invoke(nameof(PlayParticles), 1);
    }

    private void PlayParticles()
    {
        backgroundParticles.Play();
        CameraFollow.Shake(0.5f, 1);
    }
}

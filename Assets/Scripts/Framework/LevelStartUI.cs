using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LevelStartUI : MonoBehaviour
{
    public Transform levelStartUI;

    private void Start()
    {
        var canvasGroup = levelStartUI.GetComponent<CanvasGroup>();
        
        canvasGroup.alpha = 0;
        levelStartUI.localPosition = new Vector3(0, -100, 0);

        canvasGroup.DOFade(1, 0.5f);
        levelStartUI.DOLocalMoveY(0, 0.5f).OnComplete(() =>
        {
            levelStartUI.DOLocalMoveY(0, 1.5f).OnComplete(() =>
            {
                canvasGroup.DOFade(0, 0.5f);
                levelStartUI.DOLocalMoveY(100, 0.5f);
            });
        });
    }
}

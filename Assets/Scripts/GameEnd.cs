using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yakanashe.Wiper;

public class GameEnd : MonoBehaviour
{
    public Transition wiper;
    public string sceneName;
    public CanvasGroup menu;
    private void OnTriggerEnter2D(Collider2D other)
    {
        LoadScene();
    }

    public void OpenMenu()
    {
        menu.DOFade(1, 0.5f);
    }

    public void LoadScene()
    {
        wiper.In(0, () =>
        {
            SceneManager.LoadScene(sceneName);
        });
    }
}

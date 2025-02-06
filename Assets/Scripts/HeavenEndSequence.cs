using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Yakanashe.Wiper;

public class HeavenEndSequence : MonoBehaviour, IPointerDownHandler
{
    public GameObject[] dialogue;
    private int index = -1;

    public Animator handAnim, playerAnim;

    public GameObject endMenu;
    public Transition wiper;
    public TMP_Text endText;

    private float time;

    private void Start()
    {
        time = 0;
        if(Timer.instance) time = Timer.instance.GetTime();
        foreach (var text in dialogue)
        {
            text.SetActive(false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        index++;
        if (index == dialogue.Length)
        {
            PlayEndAnimation();
            return;
        }
        
        for (int i = 0; i < dialogue.Length; i++)
        {
            dialogue[i].SetActive(false);
            
            if (index != i) continue;
            dialogue[i].SetActive(true);
        }
    }

    private void PlayEndAnimation()
    {
        foreach (var text in dialogue)
        {
            text.SetActive(false);
        }

        index = 696969;
        playerAnim.Play("PlayerHeaven");
        handAnim.Play("HandOfGod");
        
        Invoke(nameof(OpenEndMenu), 8);
    }

    private void OpenEndMenu()
    {
        var shotCount = 0;
        if(Timer.instance) shotCount = Timer.instance.shotCount;
        endText.text = $"Time: {time}\nShots: {shotCount}";
        endMenu.SetActive(true);
    }
    
    public void LoadMenu()
    {
        wiper.In(0, () =>
        {
            SceneManager.LoadScene("Menu");
        });
    }
}

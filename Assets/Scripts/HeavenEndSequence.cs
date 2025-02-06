using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class HeavenEndSequence : MonoBehaviour, IPointerDownHandler
{
    public GameObject[] dialogue;
    private int index = -1;

    private void Start()
    {
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
        SceneManager.LoadScene("Menu");
    }
}

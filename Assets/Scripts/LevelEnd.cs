using UnityEngine;
using UnityEngine.SceneManagement;
using Yakanashe.Wiper;

public class LevelEnd : MonoBehaviour
{
    public Transition wiper;
    public string sceneName;
    private void OnTriggerEnter2D(Collider2D other)
    {
        LoadScene();
    }

    public void LoadScene()
    {
        wiper.In(0, () =>
        {
            SceneManager.LoadScene(sceneName);
        });
    }
}

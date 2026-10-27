
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButton : MonoBehaviour
{
    public void Retry()
    {
        SceneManager.LoadScene("ゲーム");
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene("タイトル画面");
    }
}

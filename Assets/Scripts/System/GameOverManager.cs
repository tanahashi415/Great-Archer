using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public static GameObject panel;

    public GameObject gameOverPanel;

    void Start()
    {
        panel = gameOverPanel;
    }

    public static void GameOver()
    {
        panel.SetActive(true);
    }

    public void Retry()
    {
    }

    public void ToTitle()
    {
    }
}

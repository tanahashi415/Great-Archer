using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameObject panel;

    public GameObject gameOverPanel;

    void Start()
    {
        panel = gameOverPanel;
        panel.SetActive(false);
    }

    public static void GameOver()
    {
        // SE再生
        SoundManager.instance.PlaySE(SoundManager.instance.gameOverSE, 2.0f);
        panel.SetActive(true);
        TextMeshProUGUI textBox = panel.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>();
        textBox.text = "GAME OVER";
    }

    public static void StageClear()
    {
        // SE再生
        SoundManager.instance.PlaySE(SoundManager.instance.stageClearSE, 1.0f);
        panel.SetActive(true);
        TextMeshProUGUI textBox = panel.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>();
        textBox.text = "STAGE CLEAR!!";
    }

    public void Retry()
    {
        SoundManager.instance.PlaySE(SoundManager.instance.clickSE, 2.0f);
        SceneManager.LoadScene (SceneManager.GetActiveScene().name);
    }

    public void ToTitle()
    {
        SoundManager.instance.PlaySE(SoundManager.instance.clickSE, 2.0f);
        SceneManager.LoadScene("Title");
    }
}

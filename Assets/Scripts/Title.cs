using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public GameObject creditPanel;
    public GameObject tutorialPanel;

    public void IsStart()
    {
        SoundManager.instance.PlaySE(SoundManager.instance.clickSE, 2.0f);
        SceneManager.LoadScene("Stage1");
    }

    public void Tutorial()
    {
        SoundManager.instance.PlaySE(SoundManager.instance.clickSE, 2.0f);
        tutorialPanel.SetActive(true);
    }

    public void Credit()
    {
        SoundManager.instance.PlaySE(SoundManager.instance.clickSE, 2.0f);
        creditPanel.SetActive(true);
    }

    public void Exit()
    {
        SoundManager.instance.PlaySE(SoundManager.instance.clickSE, 2.0f);
        creditPanel.SetActive(false);
        tutorialPanel.SetActive(false);
    }
}

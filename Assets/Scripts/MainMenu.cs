using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource ButtonSound;
    public GameObject HowToPlayMenu;

    public void StartGame()
    {
        ButtonSound.pitch = Random.Range(1f, 2f);
        ButtonSound.Play();
        SceneManager.LoadScene(1);
    }

    public void HowToPlay()
    {
        ButtonSound.pitch = Random.Range(1f, 2f);
        ButtonSound.Play();
        HowToPlayMenu.SetActive(true);
    }

    public void BackButton()
    {
        ButtonSound.pitch = Random.Range(1f, 2f);
        ButtonSound.Play();
        HowToPlayMenu.SetActive(false);
    }

    public void QuitGame()
    {
        ButtonSound.pitch = Random.Range(1f, 2f);
        ButtonSound.Play();
        Application.Quit();
    }
}

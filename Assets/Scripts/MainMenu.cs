using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource ButtonSound;

    public void StartGame()
    {
        ButtonSound.pitch = Random.Range(1f, 2f);
        ButtonSound.Play();
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        ButtonSound.pitch = Random.Range(1f, 2f);
        ButtonSound.Play();
        Application.Quit();
    }
}

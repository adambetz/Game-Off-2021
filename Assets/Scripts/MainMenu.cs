using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource audioSource;

    public void StartGame()
    {
        audioSource.pitch = Random.Range(1f, 2f);
        audioSource.Play();
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        audioSource.pitch = Random.Range(1f, 2f);
        audioSource.Play();
        Application.Quit();
    }
}

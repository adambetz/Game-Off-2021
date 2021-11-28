using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneMenu : MonoBehaviour
{
    public GameObject Notification;
    public TextMeshProUGUI NotificationTitle;
    public TextMeshProUGUI NotificationBodyText;

    public Transform antHolder;

    public TextMeshProUGUI antText;
    public TextMeshProUGUI dirtText;

    public float numberOfAnts = 0;
    public float numberOfDirt = 0;

    public GameObject PauseMenu;

    public TextMeshProUGUI Task1;
    public TextMeshProUGUI Task2;
    public TextMeshProUGUI Task3;
    public TextMeshProUGUI Task4;
    public TextMeshProUGUI Task5;

    public int Goal1;
    public int Goal2;
    public int Goal3;
    public int Goal4;
    public int Goal5;

    public float[] Goals1;
    public float[] Goals2;
    public float[] Goals3;
    public float[] Goals4;
    public float[] Goals5;

    public string Goal1Title;
    public string Goal2Title;
    public string Goal3Title;
    public string Goal4Title;
    public string Goal5Title;

    public string[] Goals1BodyText;
    public string[] Goals2BodyText;
    public string[] Goals3BodyText;
    public string[] Goals4BodyText;
    public string[] Goals5BodyText;

    public void addAnt()
    {
        numberOfAnts += 1;
        antText.text = "Ants: " + numberOfAnts;
        Task1.text = "Population " + numberOfAnts + "/" + Goals1[Goal1];

        if (numberOfAnts >= Goals1[Goal1])
        {
            StartCoroutine(SendNotification(Goal1Title, Goals1BodyText, Goal1));
            Goal1 += 1;
        }
    }

    public void addDirt()
    {
        numberOfDirt += 1;
        dirtText.text = "Dirt: " + numberOfDirt;
        Task2.text = "Home Building " + numberOfDirt + "/" + Goals2[Goal2];

        if (numberOfDirt >= Goals2[Goal2])
        {
            StartCoroutine(SendNotification(Goal2Title, Goals2BodyText, Goal2));
            Goal2 += 1;
        }
    }

    IEnumerator SendNotification(string Title, string[] goalsBodyText, int goalNumber)
    {
        if (Notification.activeSelf)
        {
            //add notificcation to que
        }

        NotificationTitle.text = Title;
        NotificationBodyText.text = goalsBodyText[goalNumber];
        Notification.SetActive(true);

        yield return new WaitForSeconds(5) { };
        
        NotificationTitle.text = "";
        NotificationBodyText.text = "";
        Notification.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseMenu.activeSelf) { PauseMenu.SetActive(false); }
            else { PauseMenu.SetActive(true); }
        }
    }

    public void ReturnToGame()
    {
        PauseMenu.SetActive(false);
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
